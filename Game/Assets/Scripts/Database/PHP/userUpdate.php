<?php
include('connection.php');

$username = $_POST['editUsername'];
$email = $_POST['editEmail'];
$password = $_POST['editPassword'];
$rank = $_POST['editRank'];
$coins = $_POST['editCoins'];
$kd = $_POST['editKD'];

$whereField = $_POST['whereField'];
$whereCondition = $_POST['whereCondition'];

$sql = "update users set username='".$username."',email='".$email."',password='".$password."',Rank='".$rank."',Coins='".$coins."',KD='".$kd."' where ".$whereField."='".$whereCondition."'";
mysqli_query($connect, $sql);

?>