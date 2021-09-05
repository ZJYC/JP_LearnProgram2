import json

JsonData = json.loads(open("N1extended.json",'r',encoding='utf-8').read())

OutputString = "$#" + "\n"
OutputString += "$DicTyp$%s#\n"%("单词")

for item in JsonData["data"]:
    #OutputString += "$JPWord$%s#\n"%item["slug"]
    OutputString += "%s\n"%item["slug"]
    #OutputString += "$JPkana$%s#\n"%item["japanese"][0]["reading"]
    #OutputString += "$CHWord$%s#\n"%item["senses"][0]["english_definitions"][0]
    #OutputString += "$#" + "\n"

open("Import.txt",'w',encoding='utf-8').write(OutputString)
