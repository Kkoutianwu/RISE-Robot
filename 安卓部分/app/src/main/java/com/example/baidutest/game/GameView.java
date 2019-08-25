package com.example.baidutest.game;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.PixelFormat;
import android.graphics.PorterDuff.Mode;
import android.graphics.PorterDuffXfermode;
import android.graphics.drawable.Drawable;
import android.util.AttributeSet;
import android.util.Log;
import android.view.MotionEvent;
import android.view.SurfaceHolder;
import android.view.SurfaceView;

import com.example.baidutest.MainActivity;
import com.example.baidutest.R;


/**
 * 负责游戏的显示，游戏的逻辑判断在Game.java中
 *
 * @author xiaolong
 */
public class GameView extends SurfaceView implements SurfaceHolder.Callback {

    private static final String TAG = "GameView";
    private static final boolean DEBUG = true;

    // 定义SurfaceHolder对象
    SurfaceHolder surfaceHolder = null;


    // 棋子画笔
    private Paint chessPaint = new Paint();
    // 棋盘画笔
    private Paint boardPaint = new Paint();
    private int boardColor;
    private float boardWidth;
    private float anchorWidth;

    // 清屏画笔
    private Paint clearPaint = new Paint();

    Bitmap mBlack = null;
    Bitmap mBlackNew = null;
    Bitmap mWhite = null;
    Bitmap mWhiteNew = null;

    int chessboardWidth; //棋盘宽度
    int chessboardHeight; //棋盘高度
    int chessSize; //棋盘尺寸

    Context mContext = null;

    private Game game;

    private Coordinate focus; //坐标
    private boolean isDrawFocus = false;
    private Bitmap bFocus;

    public GameView(Context context) {
        this(context, null);
        getHolder().addCallback(this);
    }

    public GameView(Context context, AttributeSet attrs) {
        super(context, attrs);
        getHolder().addCallback(this);
        mContext = context;
        boardColor = Color.BLACK;
        boardWidth = getResources().getDimensionPixelSize(R.dimen.boardWidth);
        anchorWidth = getResources().getDimensionPixelSize(R.dimen.anchorWidth);// 锚点宽度
        focus = new Coordinate();
        // connectionService = ConnectionService.getInstance();
        init();
    }

    private void init() {
        surfaceHolder = this.getHolder();
        surfaceHolder.addCallback(this);
        // 设置透明,像素格式
        setZOrderOnTop(true);
        surfaceHolder.setFormat(PixelFormat.TRANSLUCENT);
        chessPaint.setAntiAlias(true); //抗锯齿
        boardPaint.setStrokeWidth(boardWidth);//设置画笔宽度
        boardPaint.setColor(boardColor);//设置画笔颜色
        clearPaint.setXfermode(new PorterDuffXfermode(Mode.CLEAR));
        setFocusable(true);
    }

    /**
     * 设置游戏
     *
     * @param game
     */
    public void setGame(Game game) {
        this.game = game;
        requestLayout();
    }

    @Override
    protected void onMeasure(int widthMeasureSpec, int heightMeasureSpec) {
        // 设置高度与宽度一样
        int width = MeasureSpec.getSize(widthMeasureSpec);
        if (game != null) {
            if (width % game.getWidth() == 0) {
                float scale = ((float) game.getHeight()) / game.getWidth();
                int height = (int) (width * scale);
                setMeasuredDimension(width, height);
            } else {
                width = width / game.getWidth() * game.getWidth();
                float scale = ((float) game.getHeight()) / game.getWidth();
                int height = (int) (width * scale);
                setMeasuredDimension(width, height);
            }
        } else {
            super.onMeasure(widthMeasureSpec, heightMeasureSpec);
        }
    }

    @Override
    protected void onLayout(boolean changed, int left, int top, int right, int bottom) {
        super.onLayout(changed, left, top, right, bottom);
        if (DEBUG)
            Log.d(TAG, "left=" + left + "  top=" + top + " right=" + right + " bottom=" + bottom);
        if (game != null) {
            chessboardWidth = game.getWidth();
            chessboardHeight = game.getHeight();
            chessSize = (right - left) / chessboardWidth;
            Log.d(TAG, "chessSize=" + chessSize + " chessboardWidth="
                    + chessboardWidth + " chessboardHeight"
                    + chessboardHeight);
        }
    }

    /**
     * 绘制游戏界面
     */
    public void drawGameView() {
        Log.e(TAG, "drawGameView2");
        Canvas canvas = surfaceHolder.lockCanvas();
        if (surfaceHolder == null || canvas == null) {
            Log.e(TAG, "surfaceHolder=" + surfaceHolder + "  canvas=" + canvas);
            return;
        }
        // 清屏 ：是否可以不用清屏，用双缓冲技术实现
        canvas.drawPaint(clearPaint);
        drawChessBoard(canvas); //画棋盘
        drawChessPieces(canvas);  //画棋子
        drawFocus(canvas);
        surfaceHolder.unlockCanvasAndPost(canvas);
    }

    /**
     * @author:long
     * @date:2019/8/15
     * @description: 清屏
     */
    public void clearChessBoard() {
        Canvas canvas = surfaceHolder.lockCanvas();
        canvas.drawPaint(clearPaint);
        canvas.drawColor(Color.TRANSPARENT, Mode.CLEAR);
        surfaceHolder.unlockCanvasAndPost(canvas);
    }

    /**
     * 增加一个棋子
     */
    public boolean addChess(Coordinate coordinate) {
        if (game == null) {
            Log.e(TAG, "game can not be null");
            return false;
        }
        if (game.addChess(coordinate)) {
            drawGameView();
            return true;
        }
        return false;
    }


    /**
     * 创建棋子
     *
     * @param width  VIEW的宽度
     * @param height VIEW的高度
     * @param type   类型——白子或黑子
     * @return Bitmap
     */
    private Bitmap createChessPieces(int width, int height, int type) {
        int tileSize = width / 9;
        Bitmap bitmap = Bitmap.createBitmap(tileSize, tileSize, Bitmap.Config.ARGB_8888);
        Canvas canvas = new Canvas(bitmap);
        Drawable drawable = null;
        if (type == 0) {
            drawable = getResources().getDrawable(R.drawable.black);
        } else if (type == 1) {
            drawable = getResources().getDrawable(R.drawable.white);
        } else if (type == 2) {
            drawable = getResources().getDrawable(R.drawable.black_new);
        } else if (type == 3) {
            drawable = getResources().getDrawable(R.drawable.white_new);
        } else if (type == 4) {
            drawable = getResources().getDrawable(R.drawable.focus);
        }
        drawable.setBounds(0, 0, tileSize, tileSize);
        drawable.draw(canvas);
        return bitmap;
    }

    // 画棋盘背景
    private void drawChessBoard() {
        Canvas canvas = surfaceHolder.lockCanvas();
        if (surfaceHolder == null || canvas == null) {
            return;
        }
        drawChessBoard(canvas);
        surfaceHolder.unlockCanvasAndPost(canvas);
    }

    // 画棋盘背景
    private void drawChessBoard(Canvas canvas) {
        // 绘制锚点(中心点)
        int startX = chessSize / 2;
        int startY = chessSize / 2;
        int endX = startX + (chessSize * (chessboardWidth - 1));
        int endY = startY + (chessSize * (chessboardHeight - 1));
        // draw 竖直线
        for (int i = 0; i < chessboardWidth; ++i) {
            canvas.drawLine(startX + (i * chessSize), startY, startX + (i * chessSize), endY, boardPaint);
        }
        // draw 水平线
        for (int i = 0; i < chessboardHeight; ++i) {
            canvas.drawLine(startX, startY + (i * chessSize), endX, startY + (i * chessSize), boardPaint);
        }
        // 绘制锚点(中心点)
        int circleX = startX + chessSize * (chessboardWidth / 2);
        int circleY = startY + chessSize * (chessboardHeight / 2);
        canvas.drawCircle(circleX, circleY, anchorWidth, boardPaint);
        // 绘制四角锚点
        for (int i = 1; i <= 3; i = i + 2) {
            for (int j = 1; j <= 3; j = j + 2) {
                int x = startX + chessSize * (chessboardWidth / 4) * i;
                int y = startY + chessSize * (chessboardHeight / 4) * j;
                canvas.drawCircle(x, y, anchorWidth * 2 / 3, boardPaint);
            }
        }
    }

    // 画棋子
    private void drawChessPieces(Canvas canvas) {
        int[][] chessMap = game.getChessMap();
        for (int y = 0; y < chessMap.length; ++y) {
            for (int x = 0; x < chessMap[0].length; ++x) {
                int type = chessMap[x][y];
                if (type == Game.BLACK) {
                    canvas.drawBitmap(mBlack, x * chessSize, y * chessSize, chessPaint);
                } else if (type == Game.WHITE) {
                    canvas.drawBitmap(mWhite, x * chessSize, y * chessSize, chessPaint);
                }
            }
        }
        // 画最新下的一个棋子
        if (game.getLast() != null) {
            Coordinate last = game.getLast();
            int lastType = chessMap[last.getX()][last.getY()];
            if (lastType == Game.BLACK) {
                canvas.drawBitmap(mBlackNew, last.getX() * chessSize, last.getY() * chessSize, chessPaint);
            } else if (lastType == Game.WHITE) {
                canvas.drawBitmap(mWhiteNew, last.getX() * chessSize, last.getY() * chessSize, chessPaint);
            }
        }
    }


    @Override
    public boolean onTouchEvent(MotionEvent event) {
        /*if(game.getActive() == Constants.BLACK){
            return false;
        }
        int action = event.getAction();
        float x = event.getX();
        float y = event.getY();
        switch (action) {
            case MotionEvent.ACTION_DOWN:
                focus.setX((int) (x / chessSize));
                focus.setY((int) (y / chessSize));
                isDrawFocus = true;
                drawGameView();
                break;
            case MotionEvent.ACTION_MOVE:
                break;
            case MotionEvent.ACTION_UP:
                isDrawFocus = false;
                int newX = (int) (x / chessSize);
                int newY = (int) (y / chessSize);
                if (cancelAdd(newX, newY, focus)) {
                    addChess(focus.getX(), focus.getY(), game.getActive());
                    String msg = String.valueOf(focus.getX()) + "," + String.valueOf(focus.getY()) + "," + String.valueOf(game.getActive());
                    //connectionService.sendMessage(msg);
                } else {
                    drawGameView();
                }
                break;
            default:
                break;
        }*/
        return true;
    }

    /**
     * 判断是否取消此次下子
     *
     * @param x x位置
     * @param y y位置
     * @return
     */
    private boolean cancelAdd(float x, float y, Coordinate focus) {
        return x < focus.getX() + 3 && x > focus.getX() - 3
                && y < focus.getY() + 3 && y > focus.getY() - 3;
    }

    /**
     * 画当前框
     *
     * @param canvas
     */
    private void drawFocus(Canvas canvas) {
        if (isDrawFocus) {
            canvas.drawBitmap(bFocus, focus.getX() * chessSize, focus.getY() * chessSize, chessPaint);
        }
    }

    //当SurfaceView发生改变时回调该方法
    @Override
    public void surfaceChanged(SurfaceHolder holder, int format, int width, int height) {
        if (mBlack != null) {
            mBlack.recycle();
        }
        if (mWhite != null) {
            mWhite.recycle();
        }
        mBlack = createChessPieces(width, height, 0);
        mWhite = createChessPieces(width, height, 1);
        mBlackNew = createChessPieces(width, height, 2);
        mWhiteNew = createChessPieces(width, height, 3);
        bFocus = createChessPieces(width, height, 4);
    }


    //SurfaceView被创建时回调该方法
    @Override
    public void surfaceCreated(SurfaceHolder holder) {
        // 初始化棋盘
        if(MainActivity.mode == Constants.MODE_CHESS_INT ){
            drawChessBoard();
            Log.e(TAG, "Constants.MODE_CHESS_INT");
        }

        Log.e(TAG, "drawChessBoard()");
        Log.e(TAG, "surfaceCreated");
    }


    //SurfaceView被销毁时回调该方法
    @Override
    public void surfaceDestroyed(SurfaceHolder arg0) {
        Log.e(TAG, "surfaceDestroyed");
        //clearChessBoard();
        // Log.e(TAG,"clearChessBoard");
    }

}
