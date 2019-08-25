package com.example.baidutest.recognition;

import android.app.Activity;
import android.os.Handler;
import android.os.Message;
import android.util.Log;

import com.baidu.speech.EventListener;
import com.baidu.speech.EventManager;
import com.baidu.speech.EventManagerFactory;
import com.baidu.speech.asr.SpeechConstant;
import com.example.baidutest.AutoCheck;

import org.json.JSONObject;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.TreeMap;

public class Recognizer {

    private EventManager asr;
    private EventManager wakeup;
    private EventListener wakeupListener;
    private EventListener asrListener;

    private Activity activity;

    public Recognizer(Activity activity, EventListener asrListener, EventListener wakeupListener) {
        this.activity = activity;
        this.wakeupListener = wakeupListener;
        this.asrListener = asrListener;
    }

    public void wakeupStart() {
        if (wakeup != null) {
            wakeupStop();
        }
        wakeup = EventManagerFactory.create(activity, "wp");
        wakeup.registerListener(wakeupListener); //  EventListener 中 onEvent方法
        Map<String, Object> params = new TreeMap<>();
        params.put(SpeechConstant.ACCEPT_AUDIO_VOLUME, false);
        params.put(SpeechConstant.WP_WORDS_FILE, "assets:///WakeUp.bin");
        params.put(SpeechConstant.APP_ID, "16979598");
        params.put(SpeechConstant.APP_KEY, "4WfQ0H7oXTsetRHweBPQacaU");
        params.put(SpeechConstant.APP_NAME, "0L7o5a6F9NqBTGbc3YHroLptPsTwOFdB");
        // "assets:///WakeUp.bin" 表示WakeUp.bin文件定义在assets目录下
        checkAsr(params);
        String json = new JSONObject(params).toString();
        wakeup.send(SpeechConstant.WAKEUP_START, json, null, 0, 0);
    }

    public void wakeupStop() {
        if (wakeup == null) {
            return;
        }
        wakeup.send(SpeechConstant.WAKEUP_STOP, "{}", null, 0, 0);
        wakeup.unregisterListener(wakeupListener);
        wakeup = null;
    }

    public void asrStop() {
        //asr.send(SpeechConstant.ASR_STOP, null, null, 0, 0);
        //asr.send(SpeechConstant.ASR_CANCEL, null, null, 0, 0); // 取消识别
        if (asr == null) {
            return;
        }
        asr.unregisterListener(asrListener);
        asr = null;
    }

    public void asrStart() {
        if (asr != null) {
            asrStop();
        }
        asr = EventManagerFactory.create(activity, "asr");
        asr.registerListener(asrListener); //  EventListener 中 onEvent方法
        Map<String, Object> params = new LinkedHashMap<>();
        // 基于SDK集成2.1 设置识别参数
        params.put(SpeechConstant.ACCEPT_AUDIO_VOLUME, false);
        params.put(SpeechConstant.VAD_ENDPOINT_TIMEOUT, 0);
        params.put(SpeechConstant.APP_ID, "16979598");
        params.put(SpeechConstant.APP_KEY, "4WfQ0H7oXTsetRHweBPQacaU");
        params.put(SpeechConstant.APP_NAME, "0L7o5a6F9NqBTGbc3YHroLptPsTwOFdB");
        checkAsr(params);
        String json = new JSONObject(params).toString(); // 这里可以替换成你需要测试的json
        asr.send(SpeechConstant.ASR_START, json, null, 0, 0);
    }

    private void checkAsr(Map<String, Object> params) {
        Handler handler = new Handler() {
            @Override
            public void handleMessage(Message msg) {
                if (msg.what == 100) {
                    AutoCheck autoCheck = (AutoCheck) msg.obj;
                    synchronized (autoCheck) {
                        String message = autoCheck.obtainErrorMessage(); // autoCheck.obtainAllMessage();
                        Log.e("autoCheck", message);
                    }
                }
            }
        };
        AutoCheck autoCheck = new AutoCheck(activity, handler, false);
        autoCheck.checkAsr(params);
    }

}
