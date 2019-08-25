package com.example.baidutest.util;

import com.example.baidutest.game.Constants;
import com.example.baidutest.net.MessageContent;

import java.util.HashSet;
import java.util.Set;

public class TextAnalyzerUtil {

    public static final Set<String> ON_TEXT = new HashSet<>();
    public static final Set<String> OFF_TEXT = new HashSet<>();
    public static final Set<String> FACE_SCORE_WORDS = new HashSet<>();
    public static final Set<String> NAME = new HashSet<>();
    public static final Set<String> GAME_FRONT = new HashSet<>();
    public static final Set<String> GAME_BACK = new HashSet<>();
    public static final Set<String> HELLO = new HashSet<>();
    public static final Set<String> CALL = new HashSet<>();
    public static final Set<String> CIRCLE = new HashSet<>();

    static {
        CIRCLE.add("画圈圈");

        CALL.add("小寒");
        CALL.add("小韩");
        CALL.add("小涵");
        CALL.add("小憨");
        CALL.add("晓寒");
        CALL.add("萧寒");

        ON_TEXT.add("开始工作");
        ON_TEXT.add("开始");
        ON_TEXT.add("请开始工作");
        ON_TEXT.add("请开市工作");
        ON_TEXT.add("开市工作");

        OFF_TEXT.add("结束");
        OFF_TEXT.add("结束工作");
        OFF_TEXT.add("请结束工作");

        FACE_SCORE_WORDS.add("颜值");
        FACE_SCORE_WORDS.add("研制");
        FACE_SCORE_WORDS.add("胭脂");
        FACE_SCORE_WORDS.add("言之");
        FACE_SCORE_WORDS.add("莲子");
        FACE_SCORE_WORDS.add("延至");
        FACE_SCORE_WORDS.add("焉知");
        FACE_SCORE_WORDS.add("腌制");
        FACE_SCORE_WORDS.add("燕子");
        FACE_SCORE_WORDS.add("岩芝");

        GAME_FRONT.add("先手");
        GAME_FRONT.add("牵手");
        GAME_FRONT.add("选手");
        GAME_FRONT.add("先生");
        GAME_FRONT.add("纤手");
        GAME_FRONT.add("新手");
        GAME_FRONT.add("贤受");
        GAME_FRONT.add("携手");
        GAME_FRONT.add("限售");
        GAME_FRONT.add("显瘦");
        GAME_FRONT.add("纤瘦");

        GAME_BACK.add("后手");
        GAME_BACK.add("后首");
        GAME_BACK.add("后生");
        GAME_BACK.add("吼兽");

        NAME.add("我叫什么名字");
        NAME.add("我叫什么");
        NAME.add("我是");
        NAME.add("我是谁");
        NAME.add("我谁");
        NAME.add("请问我是谁");

        HELLO.add("你好");
        HELLO.add("您好");
        HELLO.add("hello");
        HELLO.add("你好呀");
        HELLO.add("你好啊");
    }

    public static MessageContent getMessage(String text) {
        if (ON_TEXT.contains(text)) {
            return getMessage(Constants.TYPE_CONTROL, Constants.ON_ROBOT, text);
        } else if (OFF_TEXT.contains(text)) {
            return getMessage(Constants.TYPE_CONTROL, Constants.OFF_ROBOT, text);
        } else if (FACE_SCORE_WORDS.contains(text)) {
            return getMessage(Constants.TYPE_INTERACTION, Constants.FACE_SCORE, "颜值");
        } else if (NAME.contains(text)) {
            return getMessage(Constants.TYPE_INTERACTION, Constants.NAME, text);
        } else if (GAME_FRONT.contains(text)) {
            text = "选择‘先手’";
            return getMessage(Constants.TYPE_CONTROL, Constants.CHESS_FRONT_TEXT, text);
        } else if (GAME_BACK.contains(text)) {
            text = "选择‘后手’";
            return getMessage(Constants.TYPE_CONTROL, Constants.CHESS_BACK_TEXT, text);
        } else if (Constants.WAKEUP_SUCCESS.equals(text)) {
            return getMessage(Constants.TYPE_PC_RESPONSE, Constants.WAKEUP_SUCCESS, Constants.SYSTEM_INIT_TEXT);
        } else if (HELLO.contains(text)) {
            return getMessage(Constants.TYPE_INTERACTION, Constants.HELLO, text);
        } else if (CALL.contains(text)) {
            return getMessage(Constants.TYPE_WORDS_TEXT, "", Constants.ROBOT_NAME);
        } else if (CIRCLE.contains(text)) {
            return getMessage(Constants.TYPE_WORDS_TEXT, "circle", Constants.CIRCLE);
        } else {
            return null;
        }
    }

    private static MessageContent getMessage(int type, String data, String text) {
        return new MessageContent(type, data, text);
    }

    private static boolean isName(String name) {
        return name.contains("我是") || name.contains("我叫");
    }

    public static MessageContent getMessageFromInput(String text) {
        if (isName(text) && text.length() > 3) {
            return getMessage(Constants.TYPE_INTERACTION, "nameis" + text.substring(2), text);
        } else {
            return getMessage(text);
        }
    }

/*    private static boolean checkCommand(String text){
        if(CIRCLE.contains(text) || OFF_TEXT.contains(text) || ON_TEXT.contains(text) || CALL.contains(text) ){

        }

        return false;
    }*/


}
