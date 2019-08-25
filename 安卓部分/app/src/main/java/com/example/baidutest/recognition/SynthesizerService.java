package com.example.baidutest.recognition;

import android.content.Context;
import android.os.Handler;
import android.os.HandlerThread;
import android.os.Message;
import android.util.Log;

import com.baidu.tts.client.SpeechSynthesizer;

import java.util.Map;


public class SynthesizerService {

    private static final int INIT = 1;
    private static final int SPEAK = 2;
    private static final int PAUSE = 3;
    private static final int RESUME = 4;
    private static final int STOP = 5;
    private static final int RELEASE = 11;


    private static final String TAG = "SynthesizerService";
    private Context context;
    private SpeechSynthesizer mSpeechSynthesizer;
    private HandlerThread hThread;
    private Handler tHandler;
    private Handler mainHandler;
    private static boolean isInitied = false;

    public SynthesizerService(Context context, InitConfig initConfig, Handler mainHandler) {
        this(context, mainHandler);
        initThread();
        runInHandlerThread(INIT, initConfig);
    }


    private SynthesizerService(Context context, Handler mainHandler) {
        if (isInitied) {
            throw new RuntimeException("SynthesizerService 类里面 SpeechSynthesizer还未释放，请勿新建一个新类");
        }
        this.context = context;
        this.mainHandler = mainHandler;
        isInitied = true;
    }

    private void initThread() {
        hThread = new HandlerThread("SynthesizerService");
        hThread.start();
        tHandler = new Handler(hThread.getLooper()) {
            @Override
            public void handleMessage(Message msg) {
                super.handleMessage(msg);
                switch (msg.what) {
                    case INIT:
                        InitConfig config = (InitConfig) msg.obj;
                        boolean isSuccess = init(config);
                        if (isSuccess) {
                            speak("初始化成功！");
                        } else {
                            speak("初始化失败！");
                        }
                        break;
                    case SPEAK:
                        String text = (String) msg.obj;
                        speak(text);
                        break;
                    case PAUSE:
                        pause();
                        break;
                    case RESUME:
                        resume();
                        break;
                    case STOP:
                        stop();
                        break;
                    case RELEASE:
                        release();
                        break;
                    default:
                        break;
                }
            }
        };
    }

    private boolean init(InitConfig config) {
        // 1. 获取实例
        mSpeechSynthesizer = SpeechSynthesizer.getInstance();
        mSpeechSynthesizer.setContext(context);

        // 2. 设置listener
        mSpeechSynthesizer.setSpeechSynthesizerListener(config.getListener());

        // 3. 设置appId，appKey.secretKey，为语音开发者平台上注册应用得到的App ID ,AppKey ，Secret Key
        mSpeechSynthesizer.setAppId(config.getAppId());
        mSpeechSynthesizer.setApiKey(config.getAppKey(), config.getSecretKey());

        // 4. 设置在线发声音人： 0 普通女声（默认） 1 普通男声 2 特别男声 3 情感男声<度逍遥> 4 情感儿童声<度丫丫>
        mSpeechSynthesizer.setParam(SpeechSynthesizer.PARAM_SPEAKER, "4");
        // 设置合成的音量，0-9 ，默认 5
        mSpeechSynthesizer.setParam(SpeechSynthesizer.PARAM_VOLUME, "9");
        // 设置合成的语速，0-9 ，默认 5
        mSpeechSynthesizer.setParam(SpeechSynthesizer.PARAM_SPEED, "5");
        // 设置合成的语调，0-9 ，默认 5
        mSpeechSynthesizer.setParam(SpeechSynthesizer.PARAM_PITCH, "2");
        // 初始化tts
        int result = mSpeechSynthesizer.initTts(config.getTtsMode());
        if (result != 0) {
            return false;
        }
        return true;
    }

    /**
     * 合成并播放
     *
     * @param text 小于1024 GBK字节，即512个汉字或者字母数字
     * @return
     */
    private int speak(String text) {
        Log.e(TAG, "speak text:" + text);
        return mSpeechSynthesizer.speak(text);
    }

    public void speakInThread(String text) {
        runInHandlerThread(SPEAK, text);
    }

    /**
     * 合成并播放
     *
     * @param text        小于1024 GBK字节，即512个汉字或者字母数字
     * @param utteranceId 用于listener的回调，默认"0"
     * @return
     */
    private int speak(String text, String utteranceId) {
        Log.e(TAG, "speak text:" + text);
        return mSpeechSynthesizer.speak(text, utteranceId);
    }

    /**
     * 只合成不播放
     *
     * @param text
     * @return
     */
    private int synthesize(String text) {
        return mSpeechSynthesizer.synthesize(text);
    }

    private int synthesize(String text, String utteranceId) {
        return mSpeechSynthesizer.synthesize(text, utteranceId);
    }

    private void setParams(Map<String, String> params) {
        if (params != null) {
            for (Map.Entry<String, String> e : params.entrySet()) {
                mSpeechSynthesizer.setParam(e.getKey(), e.getValue());
            }
        }
    }

    private int pause() {
        return mSpeechSynthesizer.pause();
    }

    public void pauseInThread() {
        runInHandlerThread(PAUSE);
    }

    private int resume() {
        return mSpeechSynthesizer.resume();
    }

    public void resumeInThread() {
        runInHandlerThread(RESUME);
    }

    private int stop() {
        return mSpeechSynthesizer.stop();
    }

    public void stopInThread() {
        runInHandlerThread(STOP);
    }

    public void releaseInThread() {
        runInHandlerThread(RELEASE);
        hThread.quitSafely();
    }

    /**
     * 设置播放音量，默认已经是最大声音
     * 0.0f为最小音量，1.0f为最大音量
     *
     * @param leftVolume  [0-1] 默认1.0f
     * @param rightVolume [0-1] 默认1.0f
     */
    public void setStereoVolume(float leftVolume, float rightVolume) {
        mSpeechSynthesizer.setStereoVolume(leftVolume, rightVolume);
    }

    private void release() {
        mSpeechSynthesizer.stop();
        mSpeechSynthesizer.release();
        mSpeechSynthesizer = null;
        isInitied = false;
    }

    /**
     * @author:long
     * @date:2019/8/21
     * @description: 释放为true
     */
    public boolean isRelease() {
        return !isInitied;
    }

    private void runInHandlerThread(int action) {
        runInHandlerThread(action, null);
    }

    private void runInHandlerThread(int action, Object obj) {
        Message msg = Message.obtain();
        msg.what = action;
        msg.obj = obj;
        tHandler.sendMessage(msg);
    }

}
