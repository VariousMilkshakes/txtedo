import sys
sys.path.append("modules/Lib")

true = True

def Start(messenger):
    messenger.command = "account"
    messenger.desc = "Browse saved accounts"
    return messenger

def Run(query):
    return True