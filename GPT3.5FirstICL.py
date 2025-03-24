import requests
import fitz
from openai import OpenAI

file_path = "C:\\Users\\user\\Documents\\Sem 5\\Big Data Applications and Analytics\\Individual Assignment 1\\U2100997_Individual_Assignment_1.pdf"

doc = fitz.open(file_path)
pdf_text = ""
for page in doc:
    pdf_text += page.get_text()
    
api_key = "APIKEY"

headers = {
    "Authorization" : f"Bearer {api_key}",
    "Content-Type" : "aplication/json"
}

url = "https://openrouter.ai/api/v1/chat/completions"

prompt = f"""
Extract the following fields from the document:
- Document Title
- Lecturer Name
- Semester
- Subject Name
- Student Name
- Summarize the file
- Framework used

Document:
\"\"\"{pdf_text}\"\"\"

Return the result in JSON format.
"""

data = {
    "model" : "openai/gpt-3.5-turbo-0613",
    "messages" : [
        {
            "role" : "user",
            "content" : prompt
        }
    ],
    "temperature" : 0.3
}

response = requests.post(url, headers=headers, json=data)

# Then try to access 'choices' if it exists
try:
    print(response.json()["choices"][0]["message"]["content"])
except KeyError:
    print("⚠️ 'choices' not found in the response. Check for error details above.")


