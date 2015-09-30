import sys
sys.path.append("modules/Lib")
import webbrowser
new = 2

true = True

def Start(messenger):
    messenger.command = "search"
    messenger.desc = "Search google"
    messenger.parent = "web"
    return messenger

def Run(query):
    url = "http://www.google.com/search?q=" + query
    webbrowser.open(url, new=new)
    return True