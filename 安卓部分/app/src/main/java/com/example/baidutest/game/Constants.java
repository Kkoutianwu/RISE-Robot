package com.example.baidutest.game;

public class Constants {


    public static final String ROBOT_NAME = "小憨";

    public static final String WAKEUP_SUCCESS = "success";
    public static final String CALL_TEXT = "小憨在呢！您有什么事吗？";

    public static final String NETWORK_EXCEPTION = "啊哦！网络异常，请检查一下您的网络哦！";

    public static final String ON_ROBOT = "on";
    public static final String OFF_ROBOT = "off";
    public static final String END_TASK = "end";

    public static final int CHESS_FRONT = 1;
    public static final int CHESS_BACK = 2;

    public static final String CHESS_FRONT_TEXT = "front";
    public static final String CHESS_BACK_TEXT = "back";

    public static final String FACE_SCORE = "face_score";
    public static final String NAME = "name";
    public static final String HELLO = "hello";

    public static final String MODE_CONTROL = "mod1";
    public static final int MODE_CONTROL_INT = 1;
    public static final String MODE_CHESS = "mod2";
    public static final int MODE_CHESS_INT = 2;
    public static final String MODE_POUR_WATER = "mod3";
    public static final int MODE_POUR_WATER_INT = 3;
    public static final String MODE_CRAWL = "mod4";
    public static final int MODE_CRAWL_INT = 4;
    public static final int MODE_END = 0;

    public static final int SYSTEM_STATE_INIT = 0;
    public static final int SYSTEM_STATE_PREPARE = 1;

    public static final String GAME_WINNER = "win";
    public static final String GAME_LOSE = "lose";
    public static final String GAME_DEUCE = "deuce";

    public static final int SOCKET_PORT = 3000;
    //public static final String SOCKET_IP = "192.168.1.217";
    public static final String SOCKET_IP = "192.168.0.199";

    public static final int CONNECTION_CID = 10;
    public static final int CONNECTION_TYPE = 10;
    public static final int CONNECTION_DATA_LENGTH = 10;

    public static final String WEBVIEW_URL = "http://192.168.0.199:50000/";
    //public static final String WEBVIEW_URL = "http://192.168.0.2:50000/video_feed";

    public static final int TYPE_CONTROL = 1;
    public static final int TYPE_INTERACTION = 2;
    public static final int TYPE_CHESS_COORDINATE = 3;
    public static final int TYPE_MODE = 4;
    public static final int TYPE_WORDS_TEXT = 5;
    public static final int TYPE_PC_RESPONSE = 6;
    public static final int TYPE_HEART_BEAT = 7;
    public static final int TYPE_CONFIRM = 8;
    public static final int TYPE_STATE = 9;

    public static final String SYSTEM_INIT_TEXT = "您好！我是小憨！我可以干的事可多啦！要不要来试试呢：\n" +
            "1、语音输入‘开始工作’，我就可以陪你玩啦！\n" +
            "2、语音输入‘颜值’，我就可以给你的颜值打分啦！\n" +
            "3、语音输入‘你好’，我就可以用自己的方式给你打招呼啦！";

    public static final String SYSTEM_PREPARE_TEXT = "1、您现在可以在工作台上放棋盒，我就可以陪您下五子棋啦！\n" +
            "2、也可以放矿泉水瓶和杯子，我可以倒水给你看呢！\n" +
            "3、还可以放入您左手边收纳盒里的这些东西，我可以将它们分拣至您面前的两个小收纳盒。\n" +
            "4、还可以放入游戏手柄，你就可以教我捡东西了!";

    public static final String MODE_CHESS_TEXT = "现在开始下棋，准备接受我的挑战吧，请语音选择‘先手’或者‘后手’。";

    public static final String MODE_CHESS_WORKING = "现在是下棋模式！";

    public static final String MODE_CHESS_WIN = "您真厉害，我甘拜下风！！！";

    public static final String MODE_CHESS_DEUCE = "平局！棋盘太小了，要不然我一定能赢你！";

    public static final String MODE_CHESS_LOSE = "您是真的菜，连我都下不过，哈哈哈！！！";

    public static final String MODE_CHESS_END = "下棋结束啦！你在工作台上放点别的东西吧!";

    public static final String MODE_CONTROL_BEGIN = "现在是遥控示教模式，你可以把你想要捡起的颜色放进纸杯里！";

    public static final String MODE_CONTROL_WORKING = "现在是遥控示教模式，小憨正在加油工作哦！";

    public static final String MODE_CONTROL_END = "桌面上已经没有您想要捡起来的东西啦！即将结束示教模式，接下来您可以试试放点别的东西。";

    public static final String MODE_WATER_BEGIN = "现在我准备给您倒水，需要您等一下哦！";

    public static final String MODE_WATER_WORKING = "小憨正在给您倒水呢！";

    public static final String MODE_WATER_END = "您对我的表现还满意吗？倒水结束了哦，您试试放点其他东西呢！";

    public static final String MODE_CRAWL_BEGIN = "桌面上这么多东西，好的，我来帮你把它们捡起来吧！";

    public static final String MODE_CRAWL_WORKING = "小憨正在帮您收拾东西呢！";

    public static final String MODE_CRAWL_END = "东西捡完啦！您看看还想放点啥呢！";

    public static final String HELLO_WORDS = "您好呀，我叫小憨，来自于西南大学软件研究与创新中心，我有一个超级有趣的灵魂，你想跟我交朋友吗？在聊天窗口输入你的名字（比如：我是小憨），我就可以记住你啦！";

    public static final String BUSYING = "小憨正在忙碌呢，等我结束工作后再来找我玩吧！";

    public static final String MODE_WORKING_TEXT = "在工作台上放点东西，让小憨开始工作吧！";

    public static final String MODE_END_TEXT = "小憨工作已完成，您还有什么指示呀！";

    public static final String CIRCLE = "画圈圈";

    public static final String APP_ID = "17052558";
    public static final String APP_KEY = "ziBgtjq02APAGtA5PGAvkKVM";
    public static final String SECRET_KEY = "gCmOF2t68K6kiD6V3c1G0Os53W8dAIu2";
}
