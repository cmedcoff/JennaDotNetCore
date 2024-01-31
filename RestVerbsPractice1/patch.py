import argparse
import json
from urllib.parse import urljoin
from urllib3.exceptions import InsecureRequestWarning
import sys

import requests
from requests_toolbelt.utils.dump import dump_all


requests.packages.urllib3.disable_warnings(category=InsecureRequestWarning)

ap = argparse.ArgumentParser()
ap.add_argument("id")
id = ap.parse_args().id

url = urljoin("http://localhost:5247/api/timeentries/", id)
headers = {"Content-Type": "application/json"}
data = json.dumps(
    [{"op": "replace", "path": "/description", "value": "patched description"}]
)
response = requests.patch(url, headers=headers, data=data, verify=False)
response_dump = dump_all(response)
print(response_dump.decode("utf8"))
