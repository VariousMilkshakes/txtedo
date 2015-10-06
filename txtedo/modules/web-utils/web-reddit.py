import sys
sys.path.append("modules/Lib")
import webbrowser
new = 2

true = True

def Start(messenger):
    messenger.command = "reddit"
    messenger.desc = "Find a subreddit"
    messenger.parent = "web"
    return messenger

def Run(query):
    url = "http://www.reddit.com/r/" + query
    webbrowser.open(url, new=new)
    return True