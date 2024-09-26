#엔터키안쳐져있어서 읽기불편하길래만듬
#


import re

def format_text(text):
    # Add a newline before numbers followed by a period
    formatted_text = re.sub(r'(\d+)\.', r'\n\1.', text)
    # Add a newline before '답:'
    formatted_text = re.sub(r'(답:)', r'\n\1', formatted_text)
    return formatted_text

def preprocess_text_file(input_file, output_file):
    # Read the input text file
    with open(input_file, 'r', encoding='utf-8') as file:
        text = file.read()

    # Format the text using the format_text function
    formatted_text = format_text(text)

    # Write the formatted text to the output file
    with open(output_file, 'w', encoding='utf-8') as file:
        file.write(formatted_text)

# Example usage:
input_file = 'input.txt'
output_file = '전공공부.txt'
preprocess_text_file(input_file, output_file)
