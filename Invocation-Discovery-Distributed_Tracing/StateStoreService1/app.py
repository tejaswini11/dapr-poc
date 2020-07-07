import flask
from flask import request, jsonify,Flask
import requests
import json
from flask_pymongo import PyMongo, MongoClient
from pprint import pprint
import random

app = flask.Flask(__name__)
#CORS(app)

app.config["MONGO_URI"] = "mongodb://localhost:27017/admin"


mongo = PyMongo(app)

@app.route('/',methods=['GET'])
def default():
    return "Hello World"


@app.route('/getall', methods=['GET'])
def getall():
    x =[]
    response=mongo.db.bankdetails.find()
    for document in response:
        x.append(str(document))
        print(str(document))
    return json.dumps(x)

@app.route('/getstate/<string:obj>', methods=['GET'])
def getstate(obj):
    x=[]
    response=mongo.db.bankdetails.find({"name": obj})
    for document in response:
        x.append(str(document))
    return json.dumps(x)


@app.route('/addstate', methods=['POST'])
def addstate():
    try:
       card=random.randint(55555,99999)
       balance=random.randint(1000,8000)
       conn = MongoClient('localhost',27017)
       db = conn.admin
       collection = db.bankdetails
       status=collection.insert_one({"name":"stu","card no": card,"balance":balance})
       conn.close()
       return "successful"
    except:
       return "unsuccessful"
       
app.run(host="localhost", port=8000, debug=True)

