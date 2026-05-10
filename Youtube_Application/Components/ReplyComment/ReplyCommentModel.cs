using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Youtube_Application.Components.ReplyComment.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace Youtube_Application.Components.ReplyComment
{
    [AddINotifyPropertyChangedInterface]
    public class ReplyCommentModel
    {
        public ReplyCommentType CommentType { get; set; }

        public string Text { get; set; } = "發表留言...";

        private bool _ButtonShow = false;

        public bool IsButtonShowBool
        {
            get
            {
                return _ButtonShow;
            }
            set
            {
                _ButtonShow = value;
            }
        }




        public string ReplyButtonText
        {
            get
            {
                if (CommentType == ReplyCommentType.AddSubComment)
                    return "回覆";
                else if (CommentType == ReplyCommentType.EditVidoComment)
                    return "儲存";
                else return "留言";
            }

        }
        public ICommand FocusCommand { get; set; }



        public ReplyCommentModel()
        {

            if (ReplyCommentType.AddSubComment == CommentType)
            {
                this.IsButtonShowBool = true;
                this.Text = "";
            }

            this.ApplyReplyComment = new RelayCommand<string>((x) =>
            {
                if (x != "發表留言..." || x != "")
                    this.NotifyReplyComment.Execute(x);
                this.Text = "";
                this.IsButtonShowBool = false;
            });
            this.ApplyCancelComment = new RelayCommand((x) =>
            {
                this.IsButtonShowBool = false;
                if (CommentType == ReplyCommentType.AddVideoComment)
                    this.Text = "發表留言...";
                if (CommentType != ReplyCommentType.AddVideoComment)
                    this.NotifyCancelComment.Execute(x);
            });
            this.FocusCommand = new RelayCommand<string>((x) =>
            {
                if (x == "GotFocus")
                {
                    this.IsButtonShowBool = true;
                    if (this.Text == "發表留言...")
                    {
                        this.Text = "";
                    }
                }
            });
        }
        public ICommand ApplyReplyComment { get; set; }
        public ICommand NotifyReplyComment { get; set; }
        public ICommand ApplyCancelComment { get; set; }
        public ICommand NotifyCancelComment { get; set; }

    }
}
