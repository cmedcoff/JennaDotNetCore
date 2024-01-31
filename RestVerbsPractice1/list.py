import json
import requests
from requests_toolbelt.utils.dump import dump_all
from urllib3.exceptions import InsecureRequestWarning

# Suppress the warnings from urllib3
requests.packages.urllib3.disable_warnings(category=InsecureRequestWarning)

response = requests.get("http://localhost:5247/api/v1/timeentries", verify=False)
dump = dump_all(response).decode("utf8")
print(dump)
print()
print(json.dumps(response.json(), indent=2))
