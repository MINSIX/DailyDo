import os
import torch
from transformers import AutoTokenizer, AutoModelForCausalLM
from langchain.document_loaders import PyPDFLoader
from langchain.text_splitter import RecursiveCharacterTextSplitter
from langchain.embeddings import HuggingFaceEmbeddings
from langchain.vectorstores import Chroma
from langchain.chains import RetrievalQA

# GPU 설정
os.environ["CUDA_VISIBLE_DEVICES"] = "1"

# 메인 모델 및 토크나이저 로드
model_id = 'MLP-KTLim/llama3-Bllossom'
tokenizer = AutoTokenizer.from_pretrained(model_id)
model = AutoModelForCausalLM.from_pretrained(
    model_id,
    torch_dtype=torch.bfloat16,
    low_cpu_mem_usage=True,
    device_map="auto"
)
model.eval()

# 임베딩 모델 설정
embed_model_name = "jhgan/ko-sroberta-multitask"
embeddings = HuggingFaceEmbeddings(
    model_name=embed_model_name,
    model_kwargs={'device': 'cuda'},
    encode_kwargs={'normalize_embeddings': True}
)

# PDF 파일 로드 및 처리
def load_and_process_pdfs(directory):
    documents = []
    for filename in os.listdir(directory):
        if filename.endswith('.pdf'):
            loader = PyPDFLoader(os.path.join(directory, filename))
            documents.extend(loader.load())
    
    text_splitter = RecursiveCharacterTextSplitter(chunk_size=1000, chunk_overlap=200)
    texts = text_splitter.split_documents(documents)
    return texts

# 벡터 데이터베이스 생성
def create_vector_db(texts):
    db = Chroma.from_documents(texts, embeddings)
    return db

# RAG 체인 생성
def create_rag_chain(db):
    retriever = db.as_retriever(search_kwargs={"k": 3})
    
    def rag_chain(query):
        docs = retriever.get_relevant_documents(query)
        context = "\n".join([doc.page_content for doc in docs])
        
        messages = [
            {"role": "system", "content": PROMPT},
            {"role": "user", "content": f"Context: {context}\n\nQuestion: {query}"}
        ]
        
        input_ids = tokenizer.apply_chat_template(
            messages,
            add_generation_prompt=True,
            return_tensors="pt"
        ).to(model.device)
        
        terminators = [
            tokenizer.eos_token_id,
            tokenizer.convert_tokens_to_ids("<|eot_id|>")
        ]
        
        outputs = model.generate(
            input_ids,
            max_new_tokens=2048,
            eos_token_id=terminators,
            do_sample=True,
            temperature=0.6,
            top_p=0.9,
            repetition_penalty=1.1
        )
        
        return tokenizer.decode(outputs[0][input_ids.shape[-1]:], skip_special_tokens=True)
    
    return rag_chain

# 메인 실행 부분
PROMPT = '''당신은 유용한 AI 어시스턴트입니다. 사용자의 질의에 대해 친절하고 정확하게 답변해야 합니다.
You are a helpful AI assistant, you'll need to answer users' queries in a friendly and accurate manner. you must answer in korean'''

# PDF 파일 처리 및 벡터 DB 생성
texts = load_and_process_pdfs("./data")
db = create_vector_db(texts)

# RAG 체인 생성
rag_chain = create_rag_chain(db)

# 사용자 입력 받기
instruction = input("질문을 입력하세요: ")

# RAG를 이용한 답변 생성 및 출력
answer = rag_chain(instruction)
print(answer)
