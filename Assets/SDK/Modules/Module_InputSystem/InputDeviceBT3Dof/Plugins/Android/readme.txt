版本：v1.0.0
维护：陈伟桦

一、获取蓝牙手柄帮助类
包名：com.invision.handshank
SDKHandShankManager getSDKHandShankManager(Context context)

Unity范例：
AndroidJavaClass mUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
AndroidJavaObject mAndroidActivity = mUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
AndroidJavaObject mContext = mAndroidActivity.Call<AndroidJavaObject>("getApplicationContext");
AndroidJavaClass mClazz = new AndroidJavaClass("com.invision.handshank.SDKHandShankManager");
AndroidJavaObject mSDKHandShankManager = mClazz.CallStatic<AndroidJavaObject>("getSDKHandShankManager", mContext);


二、Unity设置手柄连接状态回调
HandShankConnStateCallback包名变更为：com.invision.handshank.callback

setHandShankConnStateCallback(HandShankConnStateCallback mHandShankConnStateCallback)


三、Unity设置手柄点击事件回调
HandShankKeyEventCallback包名变更为：com.invision.handshank.callback

setHandShankKeyEventCallback(HandShankKeyEventCallback mHandShankKeyEventCallback)


四、蓝牙手柄是否已经连接（对应原来的接口 unityBTConnected）
index=0表示第一个手柄，index=1表示第二个手柄

int isHandShankConnected(int index)


五、返回xy坐标（对应原来的接口 unityGetTouchPosition）
index=0表示第一个手柄，index=1表示第二个手柄

int[] getTouchPosition(int index)


六、返回手柄厂商型号
index=0表示第一个手柄，index=1表示第二个手柄

String getManufacturerModel(int index)


七、Unity设置手柄3do数据回调（对应原来的接口 unity3DofMatrix）
index=0表示第一个手柄，index=1表示第二个手柄

float[] get3DofMatrix(int index)


八、Unity设置手柄3do数据回调（对应原来的接口 unity3Dof2Matrix）
两个手柄数据放一起
matrix01[0]为1.0f表示1号手柄有数据，数据为matrix01[1]-matrix01[16],为-1.0f表示无数据
matrix01[17]为1.0f表示2号手柄有数据，数据为matrix01[18]-matrix01[33],为-1.0f表示无数据

float[] getBoth3DofMatrix()


九、关闭手柄帮助类时候调用，比如退出程序

onDestroy()
