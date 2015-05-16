
Draft for 3J Programmers

3Dテトリス設計草案
================

3Dテトリスは、テトリスに奥行きを加えたゲームです。
playerはleap motion（もしくはkey入力）でブロックを移動・回転して、
ブロックを落とします。

\>> [完成予想動画](https://www.youtube.com/watch?v=P2lOHc8wReo)

## Flow

1. 新しいブロックの生成
2. playerによるブロックの移動と回転
3. ブロックの落下
4. ブロックプールに格納
5. 隙間なくブロックが揃った行の削除
6. 1に戻る

## Todo

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

## Objects

- __BlockEntity__
	- 各ブロックのデータ管理
	- [member] block1, block2, …, block8
		- 各ブロックの色と形を保持するprefabを用意しておく
	- [func] random_block()
		- ブロックの種類をランダムに返す

- __LeapHandAction__
	- Leapで検出した手の情報の取得と、対応するイベントの呼び出し
	- [func] connect_with\_block()
		- 移動と回転の対象となるブロックを取得する
	- [event] 手の回転 -> BlockController#{pitch, yaw, roll}_block()
	- [event] 手の下方向の加速度 -> BlockController#drop_block()

- __BlockController__
	- 落下させるブロックの集合を管理
	- [member] block_number : int
		- あらかじめ決めてあるブロックの番号（ブロックの形は8種類くらい?を想定）
	- [member] x, y : int
		- ブロックのx, y座標
	- [func] pitch_block(*direct*)
		- directは 1 or -1
		- directの方向にblockをx軸中心で90度回転する
	- [func] yaw_block(*direct*)
		- directの方向にblockをy軸中心で90度回転する
	- [func] roll_block(*direct*)
		- directの方向にblockをz軸中心で90度回転する
	- [func] drop_block()
		- ブロックを落とす処理
		- BlockPoolController#control_block()を呼び出すことで、\
		- このブロックはLeapHandAction下の管理からBlockPoolController下の管理に移る

- __BlockPoolController__
	- BlockPool（ブロックの溜まり場）を管理
	- [member] block_pool : <T\>型の3次元配列
		- 配列の型については、int型で良いのか、enumにすべきか、より多くの情報を保持できる構造体（クラス）がいいのか要検討
	- [func] control_block(*blockInfo*)
		- ブロックプール内での、ブロックの落下と当たり判定と行の削除を制御
			- hit_block()で衝突するまで落下させる
			- 衝突後、merge_block()でプールに合成する（block\_poolに格納する）
				- 落下ブロックはプールへの合成と同時に削除
			- remove_completed\_row()で揃った行の削除
			- next_phase()で次のフェーズに遷移
	- [func] hit_block(blockInfo)
		- ブロックプールの床もしくは、すでにあるブロックたちと接触した場合、trueを返す
	- [func] merge_block(blockInfo)
		- 落下ブロックを、プール内に合成する
	- [func] remove_completed\_row()
		- 隙間なくブロックが揃った行の削除
		- 消える行よりも上に積まれているブロックを一つ下に落とす
	- [func] next_phase()
		- BlockEntity#random_block()で新しいブロックを決めて、生成する
		- LeapHandAction#connect_with\_block()でleap motionとblockを接続させる







