import sys
sys.path.append("modules/Lib")
import csv

true = True

def Start(messenger):
    messenger.command = "add"
    messenger.desc = "Create a new account"
    messenger.parent = "account"
    messenger.triggers.Add("Test")
    data = {"command" : "add", "desc" : "Create a new account", "parent" : "account"}
    return messenger

def Run(query):

    return True

def Test():
    print "Hello World"