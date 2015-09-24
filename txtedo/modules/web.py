import sys
sys.path.append("modules/Lib")
import webbrowser
new = 2

true = True

def Start():
    print sys.path
    return ["web", "Jump into the cyber-nets"]

def Run(query):
    url = "http://www." + query
    webbrowser.open(url, new=new)
    return True