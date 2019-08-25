package com.example.baidutest.net;

import android.app.Activity;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.util.Log;
import android.widget.Toast;

import androidx.annotation.NonNull;

import com.example.baidutest.game.Constants;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.ConnectException;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.net.SocketAddress;
import java.net.SocketTimeoutException;
import java.util.HashSet;
import java.util.LinkedList;
import java.util.Queue;
import java.util.Set;
import java.util.Timer;
import java.util.TimerTask;


/**
 * Created by long on 2019/7/30.
 * 单例模式
 */

public class ConnectionService extends Thread {

    private static final String TAG = "ConnectionService";

    private volatile boolean isConnected = false;
    private volatile static int id = 0;
    private Set<Integer> cidSet = new HashSet<>();
    private Set<MessageContent> messageBuffer = new HashSet<>();
    private Queue<MessageContent> messageQueue = new LinkedList<>();
    private Socket socket = null;
    private SocketAddress socketAddress = new InetSocketAddress(Constants.SOCKET_IP, Constants.SOCKET_PORT);
    private InputStream inputStream = null;
    private OutputStream outputStream = null;
    private Handler messageHandler = null;
    private Thread thread = null;
    private Activity activity = null;


    public boolean isConnected() {
        return isConnected;
    }

    private ConnectionService() {
    }

    private static class ConnectionServiceHolder {
        private static ConnectionService instance = new ConnectionService();
    }

    private void setContext(Handler messageHandler, Activity activity) {
        this.messageHandler = messageHandler;
        this.activity = activity;
        if (thread == null || !thread.isAlive()) {
            startConnect();
            start();
            timer();
        }
    }


    public static ConnectionService getInstance(@NonNull Handler handler, @NonNull Activity activity) {
        ConnectionServiceHolder.instance.setContext(handler, activity);
        return ConnectionServiceHolder.instance;
    }

    private void ToastMakeText(final String text, final int time) {
        activity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                Toast.makeText(activity, text, time).show();
            }
        });
    }

    /**
     * @author:long
     * @date:2019/8/17
     * @description: 发送消息线程
     */
    @Override
    public void run() {
        while (true) {
            if (messageQueue.size() == 0) {
                continue;
            }
            MessageContent message = messageQueue.poll();
            if (message == null) {
                continue;
            }
            if (socket == null || outputStream == null || socket.isClosed()) {
                continue;
            }
            send(message);
        }
    }

    /**
     * @author:long
     * @date:2019/8/13
     * @description: 请求与服务端建立连接
     */
    private void connect() {
        while (true) {
            close();
            //ToastMakeText("网络正在连接...");
            try {
                socket = new Socket();
                socket.setKeepAlive(true);
                socket.setSoTimeout(6000); //设置读数据超时时间，0为永远等待
                socket.connect(socketAddress, 5000); //3秒连接不成功，重新连接
                inputStream = socket.getInputStream();
                outputStream = socket.getOutputStream();
                ToastMakeText("网络连接成功！", 0);
                isConnected = true;
                break;
            } catch (SocketTimeoutException se) {
                isConnected = false;
                ToastMakeText("网络连接超时，重新连接中...", 0);
                try {
                    Thread.sleep(3000);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            } catch (ConnectException ce) {
                isConnected = false;
                ToastMakeText("网络连接异常，正在重新连接...", 1);
            } catch (IOException e) {
                isConnected = false;
                e.printStackTrace();
                ToastMakeText("网络IO异常，正在重新连接...,", 1);
            }
        }
    }

    /*private void monitorStart() {
        final Thread monitorConnect = new Thread("monitorConnect") {
            @Override
            public void run() {
                double runtime = 0;
                int lastID = id;
                while (true) {
                    try {
                        sleep(500);
                        if (lastID == id) {
                            runtime += 0.5;
                        } else {
                            lastID = id;
                            runtime = 0;
                        }
                        if (runtime > 3) {
                            thread.interrupt();
                            while (thread.isInterrupted()) {//确保线程已经终止
                                sleep(1000);
                            }
                            close();
                            ConnectionService.this.start();
                            break;
                        }
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                }
            }
        };
        monitorConnect.start();
    }*/

    /**
     * @author:long
     * @date:2019/8/13
     * @description: 开启线程与服务端建立连接，并监听接收服务端发送的消息
     */
    private void startConnect() {
        if (thread == null) {
            thread = new Thread("ConnectionService") {
                @Override
                public void run() {
                    connect(); //连接服务端
                    while (true) {
                        MessageContent message = receiveMessage();  //接受消息封装为对象
                        if (message == null) {
                            continue;
                        }
                        Log.e(TAG, "接受到消息：" + message.toString());
                        if (message.isConfirm()) {
                            messageBuffer.remove(message);  //移除服务端已确认收到的消息，不返回确认消息
                            //callbackEvent(message);
                            continue;
                        }
                        //判断数据是否为有效数据
                        if (!cidSet.contains(message.getCid()) && !message.isHeartbeat()) {
                            cidSet.add(message.getCid());
                            callbackEvent(message);
                        }
                        /*if (message.isHeartbeat()) {
                            callbackEvent(message);
                        }*/
                        response(message);
                    }
                }
            };
        }
        if (!thread.isAlive()) {
            thread.start(); //启动线程
        }
    }

    /**
     * @author:long
     * @date:2019/8/16
     * @description: 定时重发未确认消息
     */
    private void timer() {
        final Timer timer = new Timer();
        timer.schedule(new TimerTask() {
            @Override
            public void run() {
                if (!socket.isClosed()) {
                    if (!messageBuffer.isEmpty()) {
                        for (MessageContent message :
                                messageBuffer) {
                            if (message.isTimeout()) {
                                sendMessage(message);
                            }
                        }
                    }
                }
            }
        }, 1000);   // 设定指定的时间time,此处为2000毫秒
    }

    /**
     * @author:long
     * @date:2019/8/15
     * @description: 确认返回
     */
    private void response(MessageContent msg) {
        if (msg.isConfirm()) {
            return;
        }
        MessageContent message = new MessageContent(msg);
        send(message);
    }


    /**
     * @author:long
     * @date:2019/8/13
     * @description: Handler发送消息给UI主线程
     */
    private void callbackEvent(MessageContent message) {
        Bundle bundle = new Bundle();
        bundle.putSerializable("message", message);
        Message msg = new Message();
        msg.setData(bundle);
        messageHandler.sendMessage(msg);
    }


    /**
     * @author:long
     * @date:2019/8/13
     * @description: 接受消息
     */
    private MessageContent receiveMessage() {
        MessageContent message = null;
        try {
            message = new MessageContent(inputStream);
        } catch (SocketTimeoutException se) {
            ToastMakeText("网络异常，正在重新连接...", 1);
            isConnected = false;
            connect();
            return null;
        } catch (IOException e) {
            isConnected = false;
            ToastMakeText("网络异常，正在重新连接...", 1);
            connect();
            return null;
        } catch (NumberFormatException ne) {
            ToastMakeText("NumberFormatException", 1);
        }
        return message;
    }

    /**
     * @author:long
     * @date:2019/8/13
     * @description: 发送消息
     */
    private void send(final MessageContent message) {
        if (!message.isConfirm()) {
            messageCache(message);
        }
        Log.e(TAG, "准备发送消息：" + message.toString());
        try {
            outputStream.write(message.toBytes());
            Log.e(TAG, "成功发送消息：" + message.toString());
        } catch (IOException e) {
            isConnected = false;
            e.printStackTrace();
            ToastMakeText("信息发送失败！", 1);
        }
    }

    public void sendMessage(MessageContent message) {
        messageQueue.offer(message);//加入消息队列
    }

    /**
     * @author:long
     * @date:2019/8/16
     * @description: 消息缓存
     */
    private void messageCache(MessageContent message) {
        if (message.getType() == Constants.TYPE_CONFIRM) {
            return;
        }
        messageBuffer.add(message);
    }

    /**
     * @author:long
     * @date:2019/8/13
     * @description: 断开连接
     */
    private void close() {
        try {
            if (inputStream != null) {
                inputStream.close();
            }
            if (outputStream != null) {
                outputStream.close();
            }
            if (socket != null && !socket.isClosed()) {
                socket.close();
            }
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            inputStream = null;
            outputStream = null;
            socket = null;
        }
    }

/*    public void closeConnection() {
        close();
        thread.interrupt();
        this.interrupt();
        thread = null;
        isConnected = false;
    }*/

}
