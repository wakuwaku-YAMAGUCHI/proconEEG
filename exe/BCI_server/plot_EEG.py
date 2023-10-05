"""
MatplotlibのグラフをCanvasに埋め込む

"""
import numpy as np
import matplotlib.pyplot as plt
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg
import sqlite3
import PySimpleGUI as sg
import pandas as pd
import datetime


# 図の情報をfigとaxに保存
fig = plt.figure(figsize=(5, 4))
ax = fig.add_subplot(111)
# 1波形のデータ数
n_flame = 200
# 表示させる波形のx軸の最小値と最大値
dis_min = 0
dis_max = n_flame

# データベースへの接続
def connectSQL():
    dbname = ('brain2.db')#データベース名.db拡張子で設定
    conn = sqlite3.connect(dbname, isolation_level=None)#データベースを作成、自動コミット機能ON
    return conn

# 引数makeがTrueならグラフ書き換え、Falseなら消去
def make_data_fig(fig, id, ch, state, date, make = True):
    # グラフを描画
    if make:
        conn = connectSQL() # SQLに接続
        # リラックスなら0、集中なら1をstに代入
        if state == 'リラックス':
            st = '0'
        else:
            st = '1'
        # SQL文を作成
        sql = 'SELECT ch' + ch + ' FROM eeg_data2 WHERE ex_id = ' + id + " AND state = " + st + " AND date = '" + date + "'" 
        #print(sql)
        df = pd.read_sql(sql, conn) # SQLからデータを読み込み、データフレームに保存
        x=df.iloc[:, 0] # データをxに代入
        ax.set_xlim(dis_min, dis_max)
        ax.plot(x) # 図として表示
        return fig, len(df)
    # グラフをクリア
    else:
        ax.cla()
        return fig


# グラフをキャンバスに描画する
def draw_figure(canvas, figure):
    figure_canvas = FigureCanvasTkAgg(figure, canvas)
    figure_canvas.draw()
    figure_canvas.get_tk_widget().pack(side='top', fill='both', expand=1)
    return figure_canvas

# 次の波形を表示
def lim_to_next():
    global dis_max, dis_min
    dis_max = dis_max + n_flame
    dis_min = dis_min + n_flame

# 前の波形を表示
def lim_to_back():
    global dis_max, dis_min
    dis_max = dis_max - n_flame
    dis_min = dis_min - n_flame

# 描画をクリアする際、limitもリセット
def lim_to_reset():
    global dis_max, dis_min
    dis_max = n_flame
    dis_min = 0

# 現在日時を読み込み
dt = datetime.datetime.now()
text_today = dt.strftime('%Y-%m-%d')
# GUIの配色を設定
sg.theme('Dark Teal 8')

# レイアウト
layout = [[sg.Button('表示',key='-display-'), sg.Button('消去',key='-clear-'), sg.Cancel(), 
          sg.Text('ID'), sg.InputText(key='-id-', default_text="20009999", size=(8,1)), 
          sg.Combo(['リラックス', '集中'], default_value="リラックス", key='-state-', size=(10,1)),
          sg.CalendarButton('日付選択', format='%Y-%m-%d', key='-button_calendar-', target='-text_date-'),
          sg.InputText( key='-text_date-', default_text=text_today, size=(10,1)),],
          [sg.Canvas(key='-CANVAS-')],
          [sg.Button('前へ',key='-back-', disabled=True), sg.Button('次へ',key='-next-', disabled=True), 
          sg.Text('Ch'), sg.Combo(['0', '1', '2', '3', '4', '5', '6', '7'], default_value="0", key='-ch-'),]
          ]

window = sg.Window('Results of BLAST of BLAIN', layout, location =(100,100), finalize=True)

# figとCanvasを関連付ける
fig_agg = draw_figure(window['-CANVAS-'].TKCanvas, fig)

while True:
    event, values = window.read()

    if event in (None, 'Cancel'):
        break
    # 表示ボタンを押したらグラフを表示
    elif event == '-display-':
        fig, n_data = make_data_fig(fig, values['-id-'], values['-ch-'], values['-state-'], values['-text_date-'], make=True)
        fig_agg.draw()
        if dis_max < n_data:
            window['-next-'].update(disabled=False)
    # クリアボタンを押したらグラフを消去
    elif event == '-clear-':
        lim_to_reset()
        fig = make_data_fig(fig, values['-id-'], values['-ch-'], values['-state-'], values['-text_date-'], make=False)
        fig_agg.draw()
    # 次へボタンを押したら次の波形を表示
    elif event == '-next-':
        lim_to_next()
        fig, n_data = make_data_fig(fig, values['-id-'], values['-ch-'], values['-state-'], values['-text_date-'], make=True)
        fig_agg.draw()
        window['-back-'].update(disabled=False) # 前へボタンをONに
        # もし表示できるデータ数のmaxに到達したら、次へボタンをOFFに
        if dis_max > n_data:
            window['-next-'].update(disabled=True)
    # 前へボタンを押したら前の波形を表示
    elif event == '-back-':
        lim_to_back()
        fig, n_data = make_data_fig(fig, values['-id-'], values['-ch-'], values['-state-'], values['-text_date-'], make=True)
        fig_agg.draw()
        window['-next-'].update(disabled=False) # 次へボタンをONに
        # 0に到達したら前へボタンをOFFに
        if dis_min <= 0:
            window['-back-'].update(disabled=True)

window.close()