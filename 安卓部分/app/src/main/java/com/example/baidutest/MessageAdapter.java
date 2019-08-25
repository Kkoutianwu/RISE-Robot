package com.example.baidutest;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.example.baidutest.game.Constants;
import com.example.baidutest.net.MessageContent;

import java.util.List;

public class MessageAdapter extends RecyclerView.Adapter<MessageAdapter.ViewHolder> {

    private List<MessageContent> msgList;

    static class ViewHolder extends RecyclerView.ViewHolder {
        LinearLayout leftLayout;
        LinearLayout rightLayout;
        TextView leftMsg;
        TextView rightMsg;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            leftLayout = itemView.findViewById(R.id.left_chat);
            rightLayout = itemView.findViewById(R.id.right_chat);
            leftMsg = itemView.findViewById(R.id.left_msg);
            rightMsg = itemView.findViewById(R.id.right_msg);
        }
    }

    public MessageAdapter(List<MessageContent> msgList) {
        this.msgList = msgList;
    }

    @NonNull
    @Override
    public MessageAdapter.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.chat_item, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull MessageAdapter.ViewHolder holder, int position) {
        MessageContent message = msgList.get(position);
        int type = message.getType();
        if (type == Constants.TYPE_PC_RESPONSE || type == Constants.TYPE_CHESS_COORDINATE || type == Constants.TYPE_MODE) {
            holder.leftLayout.setVisibility(View.VISIBLE);
            holder.rightLayout.setVisibility(View.GONE);
            holder.leftMsg.setText(message.getText());
        } else if (type == Constants.TYPE_CONTROL || type == Constants.TYPE_WORDS_TEXT || type == Constants.TYPE_INTERACTION) {
            holder.leftLayout.setVisibility(View.GONE);
            holder.rightLayout.setVisibility(View.VISIBLE);
            holder.rightMsg.setText(message.getText());
        }
    }

    @Override
    public int getItemCount() {
        return msgList.size();
    }
}
