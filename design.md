
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
		- -> ClassName#OtherFuncName()

クラスへのアクセスの記述方法はrubyの書き方を採用します

- ClassName.FuncName()
	- クラスメソッド
- ClassName#FuncName()
	- インスタンスメソッド
- ClassName::Const
	- クラス定数

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
	- [ ] ブロックを落下した直後に、新しいブロックが生成されるバグ
- ブロックプールを管理するクラス
	- [x] 落下してきたブロックをブロックプールの子要素に入れる
	- [x] 子要素であるブロックの子要素（つまりCube）を巡回して、各座標に基づいて3次元配列に格納する
	- [ ] 隙間なくブロックが揃った行の削除
	- [ ] 消えるアニメーション
- 落下予測位置の表示
	- [ ] ExpectDropPosViewerクラスの作成
- 次のブロックの表示
	- [ ] GameInfoViewerクラスの作成


Classes
-------

In the diagram that follows, the name of Camel case represent Class name.
Snake case represent specific object or method.

~~~

             show_title
            ----------->
 +-->(Title)<-----------KeyAction
 |      |    usr_action
 |      |
 |      |                                                  rotate
 |      |                                 KeyAction     ------------->
 |      |                                 LeapHandAction<-------------CameraController
 |      |                                      |          ch_key_bind
 |      |transition                            |
 |      |                                      |control    position
 |      |                                      |         +---------->ExpectDropPosViewer
 |      |                                      ∨         |
 |      |                              +----->BlockController------+
 |      |                     new_block|                           |droping_block
 |      |                              |                           |
 |      ∨                start_game    |                           ∨
 |  (MainGame)--->GameManager---->BlockEntity<---------------BlockPoolController
 |      |             |                |          create
 |      |             |                |
 |      |             |                ∨
 |      |             +---------->GameInfoViewer
 |      |                           * show remaining time
 |      |                           * show next block
 |      |
 |      |transition
 |      |
 |      ∨    show_score
 +---(Score)----------->ScoreViewer
            <-----------
             usr_action


-+|<∨∧>
~~~

- __GameManager__
	- \# ゲーム全体の管理
	- [member] handedness : right or left
		- playerの利き手の情報
	- [member] gameScore : int { get; set; }
		- ゲームスコア
	- [member] elapsedTime : int
		- ゲームの経過時間
	- [func] GameStart()
		- ゲーム開始に伴う初期化
		- -> GameInfoViewer#GameStartView() でplayerの入力を待つ
		- -> CreateNextBlock()
	- [func] GameOver()
		- ブロックプールが溢れた際のゲーム終了に伴う処理
	- [func] GameFinish()
		- 時間によるゲーム終了に伴う処理
		- elapsedTimeが一定時間になると呼ばれる

- __GameInfoViewer__
	- \# ゲームの画面レイアウトの管理
	- [func] ShowRemainingTime()
		- 残り時間の表示
	- [func] ShowNextBlock()
		- 次のブロックを表示

- __BlockEntity__
	- \# 各ブロックのデータ管理
	- [member] perfabMaxNum : int
		- 各ブロックのデータ数
	- [member] blocks : GameObject[perfabMaxNum]
		- 各ブロックの色と形を保持するprefab
	- [func] CreateRandomBlock()
		- ブロックをランダムに1つ生成する
		- 生成されたオブジェクトの名前は 'block(new)' <<< 重要な変更点

- __LeapHandAction__
	- \# Leapで検出した手の情報の取得と、対応するイベントの呼び出し
	- [func] ConnectWithBlock()
		- 移動と回転の対象となるブロックを取得する
	- [func] DisconectWithBlock()
		- 移動と回転の対象となるブロックのコントロールを中止する
	- [func] ChangeKeyBind()
		- 上下左右の移動を見ている方向に合わせて変更する
		- カメラの見ている方向は -> CameraController#WatchingDirection() を参照
	- [event] 手の移動 -> BlockController#MoveBlock()
	- [event] 手の回転 -> BlockController#{Pitch, Yaw, Roll}Block()
	- [event] 手の下方向の加速度 -> BlockController#DropBlock()
	- [event] 両手で回転操作 -> CameraController#Rotate()

- __KeyAction__ (for Debug)
	- \# Key入力の検出と、対応するイベントの呼び出し
	- See also "LeapHandAction"

- __CameraController__
	- \# カメラの座標・方向の制御
	- [func] Rotate(float *theta*)
		- 角度*theta*の分だけ回転させた位置にカメラを移動させる
	- [func] WatchingDirection() -> Enum
		- 見ている方向（東西南北のような情報）を返す
		- CameraController::North などの定数を用意

- __BlockController__
	- \# 落下させるブロックの制御
	- \# 新しく生成されたブロックのスクリプトとなる
	- [func] MoveBlock(float *x*, float *z*)
		- x, z座標の方向にブロックを移動する
	- [func] PitchBlock(int *direct*)
		- directは 1 or -1
		- directの方向にblockをx軸中心で90度回転する
	- [func] YawBlock(int *direct*)
		- directの方向にblockをy軸中心で90度回転する
	- [func] RollBlock(int *direct*)
		- directの方向にblockをz軸中心で90度回転する
	- [func] PullBlockToCenter()
		- 壁に食い込んだブロックを、壁と接触しない位置まで引き戻す
	- [func] CorrectPos() -> Vector3
		- 自身のx,z座標を四捨五入した座標を返す
	- [func] DropBlock()
		- ブロックを落とす処理
		- 重力の追加
		- -> {LeapHand, Key}Action#DisconectWithBlock()
		- -> BlockPoolController#ControlBlockPool()

- __BlockPoolController__
	- \# BlockPool（ブロックの溜まり場）の制御
	- [member] blockPool : GameObject[,,]
		- 各ブロックを位置を元に格納する3次元配列
	- [func] ControlBlockPool(GameObject *blockInfo*)
		- ブロックプール内での、ブロックの落下と当たり判定と行の削除を制御
		- -> LandBlock()
		- -> MergeBlock()
		- -> RemoveCompletedRow()
		- -> PoolIsFull()
		- -> NextPhase()
	- [func] LandBlock(GameObject *blockInfo*) -> bool
		- 落ちてきたブロックが着地した場合は、trueを返す
	- [func] MergeBlock(GameObject *blockInfo*)
		- 落下ブロックを、プール内に合成する
		- 具体的には、そのブロックをBlockPoolの子要素にする
	- [func] RemoveCompletedRow()
		- 隙間なくブロックが揃った行の削除
	- [func] PoolIsFull() -> bool
		- プールからブロックが溢れているか確認
		- 溢れている場合は -> GameManager#GameOver()
	- [func] NextPhase()
		- 次のブロックを生成するための処理
		- -> BlockEntity#CreateRandomBlock()
		- -> {LeapHand, Key}Action#ConectWithBlock()
	- [func] FillEmptyBlock(int *x*, int *y*, int *z*)
		- (x, z)の座標で、高さy未満のスペースに空のブロックを詰める
		- : BlockをバラバラにしてCubeにしたときに、各Cubeが勝手に落下しないようにするため
	- [func] RemoveEmptyBlock()
		- 全ての(x, z)の座標で、空のブロックの上にあったCubeが消えた場合、この空のブロックは必要ないので削除する

- __ExpectDropPosViewer__
	- \# ブロックの予想落下位置の表示
	- [func] ShowExpectDropPos()
		- 現在の操作対象のブロックの落下予測位置に、落とすブロックのやや透明なブロックを配置
	- [func] CloneSkeltonBlock()
		- 現在の操作対象のブロックのクローンを作成する
		- そのクローンの骨格を残して透明にする

Other
-----

ドキュメントについて、気になる点があればコメントしてください。



