# 信息模块说明（not fin）
## 组件
* Message Container 组件
![avatar](/Assets/Doc/IntroPics/MessageContainer.png)
Ending 处设置信息的顺序对于结局的影响  
Error Color 设置丢失的信息的颜色
Normal Color 设置正常的信息的颜色

* Message 组件
![avatar](/Assets/Doc/IntroPics/Message.png)
isLost 表示改信息是否丢失  
Order 需要手动设置该信息的顺序编号，以便最后判定结局  
Message Content 填入信息后可以自动填入到字体组件中

## 配置
![avatar](/Assets/Doc/IntroPics/SetMessage.png)
配置 message 时 需要保证有两个子物体，并分别包含 image 以及 TextMeshPro 组件  
（后续可能考虑加入方便的创建按钮）