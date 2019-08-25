package com.example.baidutest.game;

/**
 * 坐标类
 *
 * @author cuiqing
 */
public class Coordinate {
    private int x;
    private int y;
    private int type;

    public void setX(int x) {
        this.x = x;
    }

    public void setY(int y) {
        this.y = y;
    }

    public int getType() {
        return type;
    }

    public void setType(int type) {
        this.type = type;
    }

    public int getX() {
        return x;
    }

    public int getY() {
        return y;
    }

    public Coordinate() {

    }

    //将字符串坐标转化为Coordinate对象
    public Coordinate(String data) {
        if (data == null || "".equals(data)) {
            return;
        }
        String[] datas = data.split(",");
        if (datas.length != 3) {
            return;
        }
        int x = Integer.valueOf(datas[0]);
        int y = Integer.valueOf(datas[1]);
        int type = Integer.valueOf(datas[2]);
        if (check(x, y, type)) {
            this.x = x;
            this.y = y;
            this.type = type;
        } else {
            this.x = 0;
            this.y = 0;
            this.type = 0;
        }
    }

    private boolean check(int x, int y, int type) {
        if (x >= 0 && x <= 8 && y >= 0 && y <= 8 && (type == 1 || type == 2)) {
            return true;
        }
        return false;
    }


    public Coordinate(int x, int y, int type) {
        this.x = x;
        this.y = y;
        this.type = type;
    }

    public Coordinate(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public void set(int x, int y) {
        this.x = x;
        this.y = y;
    }

}
