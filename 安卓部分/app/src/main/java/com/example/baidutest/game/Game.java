package com.example.baidutest.game;

/**
 * 处理游戏逻辑
 */
public class Game {

    public static final int SCALE_SMALL = 9;
    public static final int SCALE_MEDIUM = 15;
    public static final int SCALE_LARGE = 19;
    public static final int BLACK = 1; //黑子
    public static final int WHITE = 2; //白子


    // 默认黑子先出
    private int active = 0; //玩家为黑子
    private int gameWidth = 0;
    private int gameHeight = 0;
    private Coordinate last = new Coordinate();
    private int[][] gameMap;

    public Game() {
        this.gameWidth = SCALE_SMALL;
        this.gameHeight = SCALE_SMALL;
        this.gameMap = new int[SCALE_SMALL][SCALE_SMALL];
    }

    public void clearGameMap() {
        this.gameMap = new int[SCALE_SMALL][SCALE_SMALL];
    }

    /**
     * 游戏宽度
     *
     * @return 棋盘的列数
     */
    public int getWidth() {
        return gameWidth;
    }

    /**
     * 游戏高度
     *
     * @return 棋盘横数
     */
    public int getHeight() {
        return gameHeight;
    }


    public boolean addChess(Coordinate c) {
        if (c.getType() != 1 && c.getType() != 2) {
            return false;
        }
        if (gameMap[c.getY()][c.getX()] == 0) {
            gameMap[c.getY()][c.getX()] = c.getType();
            last.setX(c.getY());
            last.setY(c.getX());
            changeActive();
            return true;
        }
        return false;
    }

    public Coordinate getLast() {
        return last;
    }


    public static int getFighter(int type) {
        if (type == BLACK) {
            return WHITE;
        } else {
            return BLACK;
        }
    }

    /**
     * 返回当前落子方
     *
     * @return mActive
     */
    public int getActive() {
        return active;
    }


    public void setActive(int active) {
        this.active = active;
    }

    /**
     * 获取棋盘
     *
     * @return 棋盘数据
     */
    public int[][] getChessMap() {
        if (active == 0) {
            return new int[SCALE_SMALL][SCALE_SMALL];
        }
        return gameMap;
    }


    private void changeActive() {
        if (active == BLACK) {
            active = WHITE;
        } else {
            active = BLACK;
        }
    }

    // 判断是否五子连珠
    private boolean isGameEnd(int x, int y, int type) {
        int leftX = x - 4 > 0 ? x - 4 : 0;
        int rightX = x + 4 < gameWidth - 1 ? x + 4 : gameWidth - 1;
        int topY = y - 4 > 0 ? y - 4 : 0;
        int bottomY = y + 4 < gameHeight - 1 ? y + 4 : gameHeight - 1;

        int horizontal = 1;
        // 横向向左
        for (int i = x - 1; i >= leftX; --i) {
            if (gameMap[i][y] != type) {
                break;
            }
            ++horizontal;
        }
        // 横向向右
        for (int i = x + 1; i <= rightX; ++i) {
            if (gameMap[i][y] != type) {
                break;
            }
            ++horizontal;
        }
        if (horizontal >= 5) {
            return true;
        }

        int vertical = 1;
        // 纵向向上
        for (int j = y - 1; j >= topY; --j) {
            if (gameMap[x][j] != type) {
                break;
            }
            ++vertical;
        }
        // 纵向向下
        for (int j = y + 1; j <= bottomY; ++j) {
            if (gameMap[x][j] != type) {
                break;
            }
            ++vertical;
        }
        if (vertical >= 5) {
            return true;
        }

        int leftOblique = 1;
        // 左斜向上
        for (int i = x + 1, j = y - 1; i <= rightX && j >= topY; ++i, --j) {
            if (gameMap[i][j] != type) {
                break;
            }
            ++leftOblique;
        }
        // 左斜向下
        for (int i = x - 1, j = y + 1; i >= leftX && j <= bottomY; --i, ++j) {
            if (gameMap[i][j] != type) {
                break;
            }
            ++leftOblique;
        }
        if (leftOblique >= 5) {
            return true;
        }

        int rightOblique = 1;
        // 右斜向上
        for (int i = x - 1, j = y - 1; i >= leftX && j >= topY; --i, --j) {
            if (gameMap[i][j] != type) {
                break;
            }
            ++rightOblique;
        }
        // 右斜向下
        for (int i = x + 1, j = y + 1; i <= rightX && j <= bottomY; ++i, ++j) {
            if (gameMap[i][j] != type) {
                break;
            }
            ++rightOblique;
        }
        if (rightOblique >= 5) {
            return true;
        }

        return false;
    }
}
