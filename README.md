# HyPack.Carousel
 Unity media(img/video) serial player, depend on AVPro/UniRX/Zenject 

## Summary
 Carousel can serial play images and videos order by name, image stay a while(3s or custom), video stay until finished.

### Usage
 ```C#
 carouselManager.PlayFromFirstSession(); // 從頭播放
 ``` 

 ```C#
 carouselManager.PlayNextSession(); // 播放下一節
 ``` 

 ```C#
 carouselManager.PlaySession(2); // 播放目標節 idx:2
 ``` 

 ```C#
 carouselManager.StopSession(); // 停止當前節
 ``` 

## Setup
### SetupExample_Hierarchy
![SetupExample_Hierarchy](.\Assets\Extensions\HyPack\Carousel\Demo\SetupExample_Hierarchy.png)
### SetupExample_Project
![SetupExample_Project](.\Assets\Extensions\HyPack\Carousel\Demo\SetupExample_Project.png)

1. Create SceneContext(Zenject) in scene
2. Drag CarouselManager.prefab to scene
3. Drag CarouselPresenter.prefab to scene/Canvas
4. Add some images and videos to `./Assets/StreamingAssets/CarouselRes/`, and naming those for play order