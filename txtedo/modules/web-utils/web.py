import sys
sys.path.append("modules/Lib")
import webbrowser
new = 2

true = True

def Start(messenger):
    messenger.command = "web"
    messenger.desc = "Jump into the cyber-nets"
    return messenger

def Run(query):
    url = "http://www." + query
    webbrowser.open(url, new=new)
    return True