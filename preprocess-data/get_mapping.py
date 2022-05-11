import json

mapping_file_path = "./json/CowDataMapping.json"

with open(mapping_file_path) as f:
    data = json.load(f)
    print(json.dumps(data))