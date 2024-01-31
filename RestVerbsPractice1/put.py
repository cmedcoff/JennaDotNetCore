import argparse
import json
from urllib.parse import urljoin
from urllib3.exceptions import InsecureRequestWarning

import requests
from requests_toolbelt.utils.dump import dump_all


# Suppress the warnings from urllib3
requests.packages.urllib3.disable_warnings(category=InsecureRequestWarning)

ap = argparse.ArgumentParser()
ap.add_argument("id")
id = ap.parse_args().id

url = urljoin("http://localhost:5247/api/timeentries/", id)
headers = {"Content-Type": "application/json"}
data = json.dumps(
    {
        "Id": id,
        "Description": "Test PUT",
        "StartTime": "2020-01-01T00:00:00",
        "EndTime": "2020-01-01T00:00:00",
    }
)
response = requests.put(url, headers=headers, data=data, verify=False)
data = dump_all(response)
print(data.decode("utf8"))
