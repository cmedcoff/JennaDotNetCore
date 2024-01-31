import json
from urllib.parse import urljoin
from urllib3.exceptions import InsecureRequestWarning

import requests
from requests_toolbelt.utils.dump import dump_all

# Suppress the warnings from urllib3
requests.packages.urllib3.disable_warnings(category=InsecureRequestWarning)

url = urljoin("http://localhost:5247/api/", "timeentries")
headers = {"Content-Type": "application/json"}
data = json.dumps(
    {
        "Description": "Jenna 2",
        "StartTime": "2020-01-01T00:00:00",
        "EndTime": "2020-01-01T00:00:00",
    }
)
response = requests.post(url, headers=headers, data=data, verify=False)
response_dump = dump_all(response)
print(response_dump.decode("utf8"))
