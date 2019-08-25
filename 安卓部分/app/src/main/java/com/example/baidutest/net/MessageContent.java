package com.example.baidutest.net;

import com.example.baidutest.game.Constants;

import java.io.IOException;
import java.io.InputStream;
import java.io.Serializable;
import java.util.Objects;

public class MessageContent implements Serializable {

    private volatile static int id = 1;
    private int cid;
    private int type;
    private String data;
    private long timestamp;
    private String text = "";

    public String getText() {
        return text;
    }

    public void setText(String text) {
        this.text = text;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        MessageContent that = (MessageContent) o;
        return cid == that.cid;
    }

    @Override
    public int hashCode() {
        return Objects.hash(cid);
    }


    public boolean isTimeout() {
        long currentTime = System.currentTimeMillis();
        long mistiming = currentTime - this.timestamp;
        if (mistiming > 3000) {
            return true;
        }
        return false;
    }

    public MessageContent(int type, String data) {
        this.type = type;
        this.data = data;
        this.cid = id;
        this.timestamp = System.currentTimeMillis();
        id++;
    }

    public MessageContent(int type, String data, String text) {
        this.type = type;
        this.data = data;
        this.cid = id;
        this.text = text;
        this.timestamp = System.currentTimeMillis();
        id++;
    }


    public MessageContent(MessageContent message) {
        this.cid = message.getCid();
        this.type = Constants.TYPE_CONFIRM;
        this.data = "";
        this.timestamp = 0;
    }

    public MessageContent(String text) {
        this.text = text;
        this.data = "";
    }

    public int getCid() {
        return cid;
    }

    public int getType() {
        return type;
    }

    public MessageContent(InputStream inputStream) throws IOException {
        this.cid = readInt(Constants.CONNECTION_CID, inputStream);
        this.type = readInt(Constants.CONNECTION_TYPE, inputStream);
        int length = readInt(Constants.CONNECTION_DATA_LENGTH, inputStream);
        byte[] msg = new byte[length];
        inputStream.read(msg);
        this.data = new String(msg);
        this.timestamp = 0;

    }

    private int readInt(int n, InputStream inputStream) throws IOException {
        byte[] bytes = new byte[n];
        inputStream.read(bytes);
        return Integer.valueOf((new String(bytes)).trim());
    }

    public byte[] toBytes() {
        String id = String.valueOf(cid);
        while (id.length() < Constants.CONNECTION_CID) {
            id += " ";
        }
        String str = String.valueOf(type);
        while (str.length() < Constants.CONNECTION_TYPE) {
            str += " ";
        }
        String len = String.valueOf(data.length());
        while (len.length() < Constants.CONNECTION_DATA_LENGTH) {
            len += " ";
        }
        return (id + str + len + data).getBytes();
    }

    public boolean isHeartbeat() {
        if (type == Constants.TYPE_HEART_BEAT) {
            return true;
        }
        return false;
    }

    public boolean isConfirm() {
        if (type == Constants.TYPE_CONFIRM) {
            return true;
        }
        return false;
    }

    public void setType(int type) {
        this.type = type;
    }


    public String getData() {
        return data;
    }

    public void setData(String data) {
        this.data = data;
    }

    @Override
    public String toString() {
        return cid + "," + type + "," + data + "," + timestamp + "," + text + ",";
    }

    public boolean isEmpty() {
        if ("".equals(data) && "".equals(text)) {
            return true;
        }
        return false;
    }

    public boolean isRobot(){
        if(type == Constants.TYPE_PC_RESPONSE || type == Constants.TYPE_MODE || type == Constants.TYPE_CHESS_COORDINATE){
            return true;
        }
        return false;
    }
}
