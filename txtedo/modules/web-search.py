import sys
sys.path.append("modules/Lib")
import webbrowser
new = 2

true = True

def Start():
    print sys.path
    return ["search", "Search google", "web"]

def Run(query):
    url = "http://www.google.com/search?q=" + query
    webbrowser.open(url, new=new)
    return True