# 填色游戏板块编辑说明
## 演示
![avatar](/Doc/IntroPics/Demo1.png)
![avatar](/Doc/IntroPics/Demo2.png)
* 通过按键实现色块的填充切换，并在完成游戏后将对应的Message变为正常。  
* 黄 + 红 = 橙  
黄 + 蓝 = 绿  
蓝 + 红 = 紫  
二级颜色无法继续混合
* 可以留下多余的色块或者空缺的画布增大游戏难度

## 组件
![avatar](/Doc/IntroPics/Color
Fill.png)

* Color Filling
  Color Filling 组件是实现小游戏效果的基本组件，色块等都由该组件管理且自动生成，编辑时只需要将设置好的 .asset数据引用绑定在该组件上便可自动生成并检测游戏

* Color Filling Listener
  作为 Color Filling 小游戏的监听管理组件，当游戏完成时负责将下一组游戏数据传入 Color Filling 组件，并解锁需要解锁的信息

## 编辑数据
![avatar](/Doc/IntroPics/CreateColor.png)
右键或者通过Assets创建


* 编辑填色画布 (Color Fill Setting)
![avatar](/Doc/IntroPics/ColorFillSetting.png)
Color Fill Setting 是最基本的游戏数据模块。  
sizeX 表示横向宽度，sizeY 表示纵向宽度，Color Canvas 可用于编辑异形网格。  
Color Request 分别代表了 **白色** **红色** **橙色** **黄色** **绿色** **蓝色** **紫色** 的色彩数量的需求。
Blocks Data 中填入色块数据 Color Block可以重复利用相同的色块。


* 编辑填色色块 (Color Block)
![avatar](/Doc/IntroPics/ColorBlock.png)
内容基本同上，可编辑色块的形状和颜色