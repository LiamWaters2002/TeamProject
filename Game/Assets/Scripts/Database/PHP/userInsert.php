<?php
include('connection.php');

$username = $_POST['addUsername'];
$email = $_POST['addEmail'];
$password = $_POST['addPassword'];
$rank = $_POST['addRank'];
$coins = $_POST['addCoins'];
$kd = $_POST['addKD'];

$sql = "insert into users (username, email, password, rank, coins, KD) values ('".$username."','".$email."','".$password."','".$rank."','".$coins."','".$kd."')";
$result = mysqli_query($connect, $sql);

?>