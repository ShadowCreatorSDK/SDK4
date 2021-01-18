
namespace SC.XR.Unity {

    public class API_Module_Notice {
        public static void Show(float time){
            Module_Notice.getInstance.StartNotice(time);
        }

        public static void close() {
            Module_Notice.getInstance.StopNotice();
        }

        public static void SetNotice(string mainString, string subString, NoticeType type = NoticeType.Warning, float distance = 0.8f, AlignmentType _anchorType = AlignmentType.Center, bool isFollower = true) {
            Module_Notice.getInstance.SetNoticeInfo( mainString,subString,type ,distance, _anchorType, isFollower);
        }

        public static void SetNotice(string mainString, string subString)
        {
            Module_Notice.getInstance.SetNoticeInfo(mainString, subString);
        }


        public static void AddStrs(string mainstr,string substr)
        {
            Module_Notice.getInstance.AddStrsList(mainstr, substr);
        }
        public static void ShowMultipleNotice(float time)
        {
            Module_Notice.getInstance.StartMultipleNotice(time);
        }
        public static void CloseMultipleNotice()
        {
            Module_Notice.getInstance.StopMultipleNotice();
        }
    }
}