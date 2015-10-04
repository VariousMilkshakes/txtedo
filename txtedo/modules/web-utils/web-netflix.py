import sys
sys.path.append("modules/Lib")
import webbrowser
new = 2

true = True

def Start(messenger):
    messenger.command = "netflix"
    messenger.desc = "Search netflix"
    messenger.parent = "web"
    return messenger

def Run(query):
    url = "http://www.netflix.com/search/" + query
    webbrowser.open(url, new=new)
    return True