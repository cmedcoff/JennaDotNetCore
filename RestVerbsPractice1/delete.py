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
id_to_delete = ap.parse_args().id

url = urljoin("http://localhost:5247/api/timeentries/", id_to_delete)
response = requests.delete(url, verify=False)
response_dump = dump_all(response)
print(response_dump.decode("utf8"))
