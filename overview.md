
Draft for 3J Programmers

3Dテトリス設計草案
================

3Dテトリスは、テトリスに奥行きを加えたゲームです。
playerはleap motion（もしくはkey入力）でブロックを移動・回転して、
ブロックを落とします。

\>> [完成予想動画](https://www.youtube.com/watch?v=P2lOHc8wReo)

Policy
------

- 命名規則はunity documentationに従います
	- クラス名はキャメルケース(SomeClass)
	- メソッド名はキャメルケース(SomeMethod)
	- 変数名は先頭が小文字のキャメルケース(someVariable)
- グローバル変数は、基本的に避けるべきです
- 処理は可能な限りモジュールで分割（もしくは階層化）します
- モジュールの最終目的は、重複処理の削除です
- 各モジュール間の結合度は低くしましょう

Flow
----

1. ゲームの開始
	1. 新しいブロックの生成
	2. playerによるブロックの移動と回転
	3. ブロックの落下
	4. ブロックプールに格納
	5. 隙間なくブロックが揃った行の削除
	6. ブロックがプールから溢れていないか確認
2. ゲームの終了
3. スコアの報告

Todo
----

- ブロックの左右奥手前方向の移動
- ブロックを落とす
	- 落下速度の変化
	- ブロックの接触判定
- ブロックの回転
	- 回転した後の形を見て、壁との接触はないか確認後、回転を行う
- 隙間なくブロックが揃った行の削除
	- 消えるアニメーション
	- 消える行よりも上に積まれているブロックを一つ下に落とす
- ブロックの生成
	- 決まった形のブロックをランダムに
- 落下予測位置の表示
- 次のブロックの表示

Classes
-------

- __BlockEntity__
	- \# 各ブロックのデータ管理
	- [member] block1, block2, …, block8
		- 各ブロックの色と形を保持するprefabを用意しておく
	- [func] RandomBlock()
		- ブロックの種類をランダムに返す

- __LeapHandAction__
	- \# Leapで検出した手の情報の取得と、対応するイベントの呼び出し
	- [func] ConnectWithBlock()
		- 移動と回転の対象となるブロックを取得する
	- [event] 手の回転 -> BlockController#{Pitch, Yaw, Roll}Block()
	- [event] 手の下方向の加速度 -> BlockController#DropBlock()

- __BlockController__
	- \# 落下させるブロックの集合を管理
	- [member] blockNumber : int
		- あらかじめ決めてあるブロックの番号（ブロックの形は8種類くらい?を想定）
	- [member] x, y : int
		- ブロックのx, y座標
	- [func] MoveBlock(*x*, *y*)
		- x, y座標にブロックを移動する
	- [func] PitchBlock(*direct*)
		- directは 1 or -1
		- directの方向にblockをx軸中心で90度回転する
	- [func] YawBlock(*direct*)
		- directの方向にblockをy軸中心で90度回転する
	- [func] RollBlock(*direct*)
		- directの方向にblockをz軸中心で90度回転する
	- [func] DropBlock()
		- ブロックを落とす処理
		- BlockPoolController#ControlBlock()を呼び出すことで、\
		- このブロックはLeapHandAction下の管理からBlockPoolController下の管理に移る

- __BlockPoolController__
	- \# BlockPool（ブロックの溜まり場）を管理
	- [member] blockPool : <T\>型の3次元配列
		- 配列の型については、int型で良いのか、enumにすべきか、より多くの情報を保持できる構造体（クラス）がいいのか要検討
	- [func] ControlBlock(*blockInfo*)
		- ブロックプール内での、ブロックの落下と当たり判定と行の削除を制御
			- HitBlock()で衝突するまで落下させる
			- 衝突後、MergeBlock()でプールに合成する（blockPoolに格納する）
				- 落下ブロックはプールへの合成と同時に削除
			- RemoveCompletedRow()で揃った行の削除
			- FullPool()でプールからブロックが溢れているか確認
				- プールから溢れているならGameManager#GameOver()を呼び出す
			- NextPhase()で次のフェーズに遷移
	- [func] HitBlock(blockInfo)
		- ブロックプールの床もしくは、すでにあるブロックたちと接触した場合、trueを返す
	- [func] MergeBlock(blockInfo)
		- 落下ブロックを、プール内に合成する
	- [func] RemoveCompletedRow()
		- 隙間なくブロックが揃った行の削除
		- 消える行よりも上に積まれているブロックを一つ下に落とす
	- [func] NextPhase()
		- BlockEntity#RandomBlock()で新しいブロックを決めて、生成する
		- LeapHandAction#ConnectWithBlock()を呼び出すことで、leap motionとblockを接続させて、\
		- 新しいブロックをLeapHandAction下の管理に移す

- __GameManager__
	- \# ゲーム全体の管理
	- [member] handedness : right or left
		- playerの利き手の情報
	- [member] gameScore : int
		- ゲームスコア
	- [member] elapsedTime : int
		- ゲームの経過時間
	- [func] GameStart()
		- ゲーム開始に伴う初期化
	- [func] GameOver()
		- 正常ではないゲーム終了に伴う処理
	- [func] GameFinish()
		- 時間によるゲーム終了に伴う処理

Other
-----

ドキュメントについて、気になる点があればコメントしてください。



