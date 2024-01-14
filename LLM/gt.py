import torch
from transformers import GenerationConfig, LlamaForCausalLM, LlamaTokenizer,AutoModelForCausalLM,AutoTokenizer
from utils.prompter import Prompter
from peft import PeftModel
import time
import fire
device = torch.device("cuda:0")
try:
    if torch.backends.mps.is_available():
        device = "mps"
except:  # noqa: E722
    pass
def main(
    base_model: str = "beomi/OPEN-SOLAR-KO-10.7B",
    lora_weights: str = "./output_Jan13",
    prompt_template: str = "",
):
    from transformers import BitsAndBytesConfig

    quantization_config = BitsAndBytesConfig(llm_int8_enable_fp_cpu_offload=True)

    prompter = Prompter(prompt_template)
    tokenizer = AutoTokenizer.from_pretrained(base_model)

    model = AutoModelForCausalLM.from_pretrained(
        base_model,
        load_in_8bit=False,
        torch_dtype=torch.float16,
        device_map="auto",
        quantization_config=quantization_config,
        offload_folder="."
    )

    model = PeftModel.from_pretrained(
        model,
        lora_weights,
        offload_dir="",
        torch_dtype=torch.float16,
    )

    model.config.pad_token_id = tokenizer.pad_token_id = 0
    # model.config.bos_token_id = 1
    # model.config.eos_token_id = 2
    
    model.eval()

    while True:
        instruction = input("Enter instruction (type 'exit' to quit): ")
        if instruction.lower() in ['exit', 'quit', 'q']:
            break

        # Prompting for input is optional, you can modify this part as needed
       # input_text = input("Enter input text (optional): ")
        start_time = time.time()

        # Evaluate the input and print the response
        response_generator = evaluate(
            prompter, model, tokenizer, instruction
        )
     
        response = next(response_generator, None)
        
        end_time = time.time()
        extracted_text = response.split('"')[1] if '"' in response else ""
        response = extracted_text
        # Calculate and print the time taken
        elapsed_time = end_time - start_time
        print(f"Time taken: {elapsed_time:.4f} seconds")
        # Print the response
        if response:
            print("Output:", response)
        else:
            print("No response generated.")

def evaluate(prompter, model, tokenizer, instruction,  temperature=0.1,
             top_p=0.75, top_k=70, num_beams=1, max_new_tokens=512,
             repetition_penalty=4.8, stream_output=True, **kwargs):
    input=None
    prompt = prompter.generate_prompt(instruction, input)
    inputs = tokenizer(prompt, return_tensors="pt",return_token_type_ids=False)
    input_ids = inputs["input_ids"].to(device)
    generation_config = GenerationConfig(
        temperature=temperature,
        top_p=top_p,
        top_k=top_k,
        num_beams=num_beams,
        repetition_penalty=float(repetition_penalty),
        **kwargs,
    )

    generate_params = {
        "input_ids": input_ids,
        "generation_config": generation_config,
        "return_dict_in_generate": True,
        "output_scores": True,
        "max_new_tokens": max_new_tokens,
  
    }

    with torch.no_grad():
        generation_output = model.generate(**generate_params )

    s = generation_output.sequences[0]
    output = tokenizer.decode(s)
   
    
    yield prompter.get_response(output)

if __name__ == "__main__":
    fire.Fire(main)
    main()