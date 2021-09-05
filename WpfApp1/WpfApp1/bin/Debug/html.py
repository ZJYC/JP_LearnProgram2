# -*- coding:UTF-8 -*-
import requests
from bs4 import BeautifulSoup
from urllib import parse
import time
import random
import os

def GetWord(Input = ""):
    FileNameToWrit = Input+".txt"
    if FileNameToWrit in os.listdir():
        print(Input + "Already existed....")
        return
    target = 'https://nekodict.com/words?q='+parse.quote(Input)
    req = requests.get(url=target)
    open(FileNameToWrit,'w',encoding='utf-8').write(str(req.text))
    print(Input + "OK")
    SleepDelay = random.randint(10,30)
    print("Will sleep %d S "%SleepDelay,end="")
    for i in range(SleepDelay):
        time.sleep(1)
        print(".",end="")
    print("")

Alllines = open("Import.txt",'r',encoding='utf-8').readlines()
for line in Alllines:
    line = line.replace("\n","")
    GetWord(line)