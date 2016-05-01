
Design for 3J Programmers

3Dテトリス設計詳細
================

3dテトリスの基本部分の設計


Policy
------

- 命名規則はunity documentationに従います
	- クラス名はキャメルケース(SomeClass)
	- メソッド名はキャメルケース(SomeMethod)
	- 変数名は先頭が小文字のキャメルケース(someVariable)
- メソッドは次のように表します
	- instance.FuncName() : インスタンスメソッド

次の設計の書き方は独自のものです

- __ClassName__
	- \# クラスの説明
	- [member] varName : type
		- 変数の説明
	- [func] FuncName() -> return_type
		- 関数の説明
	- [func] FuncName(type *args*)
		- 引数は斜体にする。返り値がない場合は`->`を省略
	- [func] FuncName()
		- 関数内で別の関数を呼ぶときは`->`を追加
		- -> ClassName.OtherFuncName()


Todo
----

- ブロックの生成
	- [x] 決まった形のブロックをランダムに
- ブロックの左右奥手前方向の移動
	- [x] 壁に食い込まないようにする
	- [x] もしくは壁に食い込んだ分、位置を戻す
- ブロックの回転
	- [x] 回転した際に壁に食い込む場合は、適切な位置まで移動させる
- ブロックを落とす
	- [x] ブロックを落下した直後に、新しいブロックが生成されるバグ
- ブロックプールを管理するクラス
	- [x] 落下してきたブロックをブロックプールの子要素に入れる
	- [x] 子要素であるブロックの子要素（つまりCube）を巡回して、各座標に基づいて3次元配列に格納する
	- [x] 隙間なくブロックが揃った行の削除
	- [x] 消えるアニメーション
	- [x] 消した分の得点計算
- 落下予測位置の表示
	- [x] ExpectDropPosViewerクラスの作成
- Play中の画面
	- [x] 残り時間の表示
	- [x] スコアの表示
	- [x] 次に出すブロックの表示
- Sceneの作成
	- [x] タイトル画面
	- [x] スコア画面
- Leap Motionへの対応
	- [x] ブロック操作のLeap化
- エコらしいテクスチャ
	- [x] 可燃
	- [x] 不燃
	- [x] 生ごみ
	- [x] カン, ビン
- エコらしい操作
	- [x] ブロックをつぶす
	- [x] ブロックを振る
		- 水が出るエフェクト


Classes
-------

In the diagram that follows, the name of Camel case represent Class name.
Snake case represent specific object or method.

~~~

           show_title
          ----------->
   (Title)<-----------KeyAction
    ∧  |   usr_action
    |  |
    |  |transition
    |  |                                        rotate
    |  |                       KeyAction     ------------->
    |  |                       LeapHandAction<-------------CameraController
    |  |                            |          ch_key_bind
    |  |                            |
    |  |                            |control    position
    |  |                            |         +---------->ExpectDropPosViewer
    |  |                            |         |
    |  |                            ∨         |
    |  |                          BlockController---------+
    |  |                              ∧  |                |
    |  |                     new_block|  |create          |droping_block
    |  |                              |  |                |
    |  ∨                  start_game  |  ∨                ∨             delete
   (MainGame)--->GameManager------>BlockEntity      BlockPoolController------->CubeInfo
    |  |             |  ∧             |                   |
    |  |             |  |             |next_block         |game_over
    |  |             |  |             |                   |
    |  |             |  +-------------|-------------------+
    |  |             |                |
    |  |             |                ∨
    |  |             +---------->GameInfoViewer
    |  |          remaining_time
    |  |
    |  |
    |  |transition
    |  |
    |  ∨   show_score
   (Score)----------->ScoreViewer
          <-----------
           usr_action



        BaseAction
            |
            |extends
            ∨
        PlayerAction
            |
            |extends
            +-------------+
            |             |
            ∨             ∨         use
        KeyAction    LeapHandAction---->LeapHands


-+|<∨∧>
~~~

- __GameManager__
	- \# ゲーム全体の管理
	- public
	- [member] handedness : string
		- playerの利き手の情報
		- "right" または "left"
	- [member] score : int
		- ゲームスコア
	- [member] remainingTime : int
		- ゲームの残り時間
	- [func] GameStart()
		- ゲーム開始に伴う初期化
		- -> BlockEntity.CreateNextBlock()
	- [func] GameOver()
		- ブロックプールが溢れた際のゲーム終了に伴う処理
	- [func] GameFinish()
		- 時間によるゲーム終了に伴う処理
		- remainingTimeが0になると、この関数が呼ばれる

- __GameInfoViewer__
	- \# ゲームの画面レイアウトの管理
	- private
	- [func] OnGUI()
		- 消した行数の表示
		- 獲得したスコアの表示
		- 残り時間の表示
		- -> ShowNextBlock()
	- [func] ShowNextBlock()
		- 次のブロックを表示

- __BlockEntity__
	- \# 各ブロックのデータ管理
	- public
	- [member] perfabMaxNum : int
		- 各ブロックのデータ数
	- [member] blocks : GameObject[perfabMaxNum]
		- 各ブロックの色と形を保持するprefab
	- [func] CreateRandomBlock()
		- ブロックをランダムに1つ生成する
		- 生成されたオブジェクトの名前は 'block(new)'
		- -> KeyAction.ConnectWithBlock()

- __LeapHandAction__
	- \# Leapで検出した手の情報の取得と、対応するイベントの呼び出し
	- public
	- [func] ConnectWithBlock()
		- 移動と回転の対象となるブロックのコントロールを開始する
	- [func] DisconectWithBlock()
		- 移動と回転の対象となるブロックのコントロールを中止する
	- [event] 手の移動 -> BlockController.MoveBlock()
	- [event] 手の回転 -> BlockController.{Pitch, Yaw, Roll}Block()
	- [event] 手の下方向の加速度 -> BlockController.DropBlock()
	- [event] 両手で回転操作 -> CameraController.RotateCam()

- __KeyAction__ (for Debug)
	- \# Key入力の検出と、対応するイベントの呼び出し
	- See also "LeapHandAction"

- __CameraController__
	- \# カメラの座標・方向の制御
	- public
	- [func] RotateCam(float *theta*)
		- 角度*theta*の分だけ回転させた位置にカメラを移動させる
	- private
	- [func] Update()
		- 常にBlockPoolの中心を見る

- __BlockController__
	- \# 落下させるブロックの制御
	- \# 新しく生成されたブロックのスクリプトとなる
	- public
	- [func] MoveBlock(float *x*, float *z*)
		- x, z座標の方向にブロックを移動する
	- [func] PitchBlock(Vector3 *direct*)
		- directの方向にblockをx軸中心で90度回転する
	- [func] YawBlock(Vector3 *direct*)
		- directの方向にblockをy軸中心で90度回転する
	- [func] RollBlock(Vector3 *direct*)
		- directの方向にblockをz軸中心で90度回転する
	- [func] DropBlock()
		- ブロックを落とす処理
		- 重力の追加
		- 落下後、Poolに着地したタイミングで、OnCollisionEnter()が呼ばれる
	- [func] GetCorrectPosition() -> Vector3
		- 自身のx,z座標を四捨五入した座標を返す
	- private
	- [func] Rotate(float *x*, float *y*, float *z*)
		- 世界軸を中心にブロックの回転を行う
	- [func] OnCollisionEnter()
		- colliderに当たり判定があると、この関数が呼ばれる
		- -> {LeapHand, Key}Action.DisconectWithBlock()
		- -> BlockPoolController.ControlBlockPool()
		- -> BlockEntity.CreateRandomBlock()
		- -> {LeapHand, Key}Action.ConectWithBlock()
	- [func] roundXZ(Vector3 vector) -> Vector3
		- 引数のvectorのx,z成分だけ四捨五入する
	- [func] CorrectPosition() -> Vector3
		- 自身のx,z座標を四捨五入した座標に移動させる
	- [func] CorrectDirection() -> Vector3
		- x,z要素を四捨五入したVector3を返す
	- [func] FixPosition()
		- 壁に食い込んだブロックを、壁と接触しない位置まで引き戻す


- __BlockPoolController__
	- \# BlockPool（ブロックの溜まり場）の管理
	- private
	- [member] POOL\_X, POOL\_Y, POOL\_Z
		- Poolのサイズ
	- [member] blockPool : GameObject[,,]
		- 各ブロックを位置を元に格納する3次元配列
	- public
	- [func] ControlBlockPool(GameObject *block*)
		- ブロックプール内における作業の集まり
		- 引数は、Poolに着地したブロック
		- -> InitPool()
		- -> MergeBlock()
		- -> SearchCubePos()
		- -> FixCubePos()
		- -> RemoveCompletedRow()
	- private
	- [func] InitPool()
		- blockPoolの初期化
	- [func] MergeBlock(GameObject *block*)
		- 落下ブロックを、プール内に合成する
		- 具体的には、そのブロックをBlockPoolの子要素にする
	- [func] SearchCubePos()
		- 壁の位置を取得
		- -> SetCubePos()
	- [func] SetCubePos(Transform *obj*, Vector3 *offset*)
		- 壁の位置をoffsetとして、ブロックの位置をblockPool（3次元配列）に保存する
		- 配列に保存できない（Poolから溢れた）場合は、-> GameManager.GameOver()
	- [func] FixCubePos()
		- Pool内の全てのブロックに対して、位置のずれ（誤差）を補正する
	- [func] RemoveCompletedRow()
		- 隙間なくCubeが揃った行の削除
		- Cubeを削除する前に、削除されるCubeの上にあるCubeを全てDummyParentの子要素にする
		- 最後に、DummyParentに対して重力を与える

- __DummyParent__
	- \# Pool内の削除される行の上にあるCubesを管理する
	- public
	- [member] isLanded : bool
		- CubesがPoolに着地した場合はtrueを保持する
	- [func] StartDropping()
		- isLandedをfalseにする
		- 自身に対して、重力を与える
	- [func] FinishDropping()
		- isLandedをfalseにする
		- 自身に働いている重力を外す
		- 自身の子要素であるCubesを、Poolに戻す
	- private
	- [func] Setup()
		- 初期設定を行う
	- [func] OnCollisionEnter()
		- Poolに着地した場合、isLandedをtrueにする


- __ExpectDropPosViewer__
	- \# ブロックの予想落下位置の表示
	- \# ブロック生成時に付加されるスクリプト
	- public
	- [func] StopSync()
		- 落下予測位置の表示するとき、操作中のブロックの動きに合わせるのを停止する
	- [func] StopShowing()
		- 落下予測位置の表示を停止する
	- private
	- [func] roundXZ(Vector3 vector) -> Vector3
		- ベクトルのx,z成分を四捨五入したベクトルを返す
	- [func] ExpectDropPos(Vector3 position) -> Vector3
		- 操作中のブロックの座標*position*を元に、落下予測位置の座標を返す
	- [func] CloneSkeltonBlock()
		- この関数は、Start()にて1度だけ呼び出される
		- 現在の操作対象のブロックの落下予測位置に、半透明なブロックを配置
		- GameObject *showDropPosBlock* が、ブロックのインスタンスの実体
	- [func] SyncOriginBlock()
		- 操作中のブロックが移動・回転した際に、落下予想位置を表示するブロックの位置・形を更新する
		- この関数はUpdate()によって呼び出される


Other
-----

ドキュメントについて、気になる点があればコメントしてください。
