import sqlite3
import socket
from threading import Timer
import time
import struct
import datetime
import pandas as pd



# グローバル変数
HOST = "localhost"  # 接続先ホストの名前
#HOST = "127.0.0.1" # 接続先ホストの名前
PORT = 50000        # ポート番号
BUFSIZE = 4096      # 受信バッファの大きさ
NumHeaderRows = 4   # ヘッダーの行数

# サーバからのメッセージの受信(fはファイルポインタ)
def getMessage():
    client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)# ソケットの作成
    client.connect((HOST, PORT)) # サーバとの接続
    data = client.recv(BUFSIZE)
    client.close()    # コネクションのクローズ
    return data

"""  テーブル作成部分
・create table テーブル名（作成したいデータカラム）というSQL文でテーブルを宣言
・sub_id：被験者のID、ex_id：実験id、state:0ならリラックス、1なら集中、ch0~7：波形のデータ、oclock：何時か、date：何日か
　sub_idは1000XXXX、実験idは2000XXXX、eegdataは浮動小数点、oclockは0～23の整数、dateは20230923の形式
　　※NULL, INTEGER(整数), REAL(浮動小数点), TEXT(文字列), BLOB(バイナリ)の5種類
"""
def createSQL(cursor, conn):
    sql = """CREATE TABLE IF NOT EXISTS eeg_data2(sub_id INT, ex_id INT, state INT, ch0 DECIMAL, ch1 DECIMAL, ch2 DECIMAL, ch3 DECIMAL, ch4 DECIMAL, ch5 DECIMAL, ch6 DECIMAL, ch7 DECIMAL, oclock time, date DATE)"""
    cursor.execute(sql)#executeコマンドでSQL文を実行
    conn.commit()#データベースにコミット(Excelでいう上書き保存。自動コミット設定なので不要だが一応・・)


# データベースへの接続
def connectSQL():
    dbname = ('brain2.db')#データベース名.db拡張子で設定
    conn = sqlite3.connect(dbname, isolation_level=None)#データベースを作成、自動コミット機能ON
    return conn



"""
レコードを追加する場合はinsert文を使う。
SQLインジェクションという不正SQL命令への脆弱性対策でpythonの場合は「？」を使用して記載するのが基本。
"""
# SQL文を作成する用の関数、INSERT文で用いる
def makeStrData(sid, eid, state, serverData):
    data = ((sid, eid, state))#挿入するレコードを指定
    serverData = serverData.decode(encoding='utf-8') #バイト列を文字列に変換
    serverData = serverData.split(',') # カンマ区切りの文字列をリストに変換
    serverData.pop(0) # 先頭(Sample Index)は消す
    # Ch0~7をdataに加えていく
    for sd in serverData:
        data = data + (float(sd),)
    # 現在時刻(何時かだけ)と日付を記憶
    dt_now = datetime.datetime.now()
    data = data + (dt_now.strftime('%H:%M:%S.%f'), dt_now.strftime('%Y-%m-%d'))
    return data
# データを挿入
def insertSQL2(cursor, conn, sid, eid, state, serverData):
    sql = """INSERT INTO eeg_data2 VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"""#?は後で値を受け取るよという意味
    data = ((sid, eid, state))#挿入するレコードを指定
    cursor.execute(sql, makeStrData(sid, eid, state, serverData))#executeコマンドでSQL文を実行
    conn.commit()#コミットする


#for i in range(20):
    #getMessage()
    #time.sleep(0.05)

conn = connectSQL()
cursor = conn.cursor() #カーソルオブジェクトを作成
#createSQL(cursor, conn)
for i in range(50):
    serverData = getMessage()
    insertSQL2(cursor, conn, 10009999, 20009999, 0, serverData)

# dbをread_sqlを使用してpandasとして読み出す。
df = pd.read_sql('SELECT * FROM eeg_data2', conn)
print(df)