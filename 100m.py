#SPORT GaME (100メートル走)
import pgzrun

WIDTH = 800 #画面の幅
HEIGHT = 600 #画面の高さ
GOAL = 1200

#ゲームの初期化
def init():
    global player,time,countdown
    global gameover,titlemode,keyold
    #スプライトを作成
    player = Actor('player_stand',(150,400))
    player.anime = 0#アニメ用
    player.speed = 0#速度
    player.time = 0#タイム
    player.length = 0#走った距離
    keyold =False 
    countdown = 3
    gameover = 0
    titlemode = True
    sounds.audiostock_47304.play()
def draw():
        message = ['G O !','READY...','READY','']
        if titlemode == True:
            screen.fill('RED')
            screen.draw.text('SPORT GAME(100Metres)',
            left = 150,top =250,fontsize = 64,
            color='BLUE',owidth=2,ocolor='WHITE')
        else:
            screen.blit('background',(0,0))
            screen.draw.filled_rect(Rect((0,250),
            (WIDTH,100)),'DARKGREEN')
            screen.draw.filled_rect(Rect((0,350),
            (WIDTH,250)),'ORANGE')
            screen.blit('track1',
            ((player.x - player.length),player.y))
            player.draw()
            if countdown >= 0:
                screen.draw.text(message[int(countdown)],
                    left=300,top=50,fontsize=64,
                    color='RED',owidth=2,ocolor='WHITE')
            diffrence =  1 - (player.length / GOAL)
            if diffrence < 0 : diffrence = 0
            diffrence = (int)(100 * diffrence)
            screen.draw.text(
                'TIME : '+'{:6.3f}'.format(player.time),
                left = 0,top=520,fontsize=64,
                scolor='RED',owidth=2,ocolor='RED')
            screen.draw.text(
                'Up to the Goal : '+(str)(diffrence) + 'm',
                left = 300,top=520,fontsize=64,
                scolor='RED',owidth=2,ocolor='RED')
            if gameover == 0:
                screen.draw.text('[SPACE]:RUN',left = 300,
                top=285,fontsize=40,color='WHITE')
            else:
                screen.draw.text('FINISH!',
                left=300,top=50,fontsize=64,color='RED',owidth=2,ocolor='WHITE')
def update():
    global player,countdown,titlemode
    global gameover,keyold

    if titlemode == True:
        if keyboard.space: 
            titlemode = False
            sounds.audiostock_47304.stop()
        return

    if countdown >= 0:
        countdown = countdown - 0.01666
        if countdown <= 0 : sounds.audiostock_49222.play()
        return

    player.speed = player.speed -1
    if player.speed  < 0: player.speed = 0
    player.length = player.length + (player.speed / 8)
    if gameover == 0:
        player.time = player.time + 0.01666
        keynew = keyboard.space
        if keynew != keyold:
            keyold = keynew
            player.anime = (player.anime + 1) % 4
            if player.anime == 0:
                player.image ='player_walk1'
            if player.anime == 2:
                player.image ='player_walk2'
            if keynew == True:
                player.speed = player.speed + 8
                if player.speed > 20: player.speed = 20
        if player.length >=GOAL: #ゴールに到達
                gameover = 1
                sounds.audiostock_49222.stop()
                sounds.audiostock_51065.play()
    else:
        gameover += 1
        if gameover > 300:
            sounds.audiostock_51065.stop()
            init()
init()
pgzrun.go()





                