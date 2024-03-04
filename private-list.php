<?php
    require 'user-header.php';
    
    
?>
    
<!-- ***** Search Area ***** -->

<!-- ***** Main Banner Area Start ***** -->
<div class="main-banner header-text" id="top">
    <div class="Modern-Slider">
      <!-- Item -->
      <div class="item">
        <div class="img-fill">
            <img src="assets/images/slide-03.jpg" alt="">
            <style>
                .text-content
                {
                    top:30%!important;
                    //left:50%!important;
                    margin:auto!important;
                    //background:white;
                    width:80%!important;
                    text-align:left;
                }
            </style>
            <div class="text-content">
                <style>
                    #ranklist {
                      font-family: Arial, Helvetica, sans-serif;
                      border-collapse: collapse;
                      width: 100%;
                      color:white;
                      box-shadow: rgba(50, 50, 93, 0.25) 0px 6px 12px -2px, rgba(0, 0, 0, 0.3) 0px 3px 7px -3px;
                    }
                    .rank
                    {
                        width:25px;
                        text-align:center;
                    }
                    #ranklist td, #ranklist th {
                      border: 1px solid #ddd;
                      padding: 8px;
                    }
                    
                    #ranklist tr:nth-child(even){//background-color: #f2f2f2;}
                    
                    #ranklist tr:hover {background: -webkit-linear-gradient(left, #25c481, #25b7c4);
  background: linear-gradient(to right, #25c481, #25b7c4);}
                    
                    #ranklist th {
                      padding-top: 12px;
                      padding-bottom: 12px;
                      text-align: left;
                      background-color: #04AA6D;
                      color: white;
                      background: -webkit-linear-gradient(left, #25c481, #25b7c4);
  background: linear-gradient(to right, #25c481, #25b7c4);
                    }
                    
                    #searchData
                    {
                        margin-top:40px;
                    }
                    #searchData option
                    {
                        color:black;
                        min-width:250px;
                    }
                    #searchData label
                    {
                        color:white;
                    }
                    #searchData #InstituteName
                    {
                        width:150px;
                        padding:3px;
                    }
                </style>
                
                <div id="searchData">
                    <!-- Start -->
                    <form class="searchForm" action="" method="get">
                        <label for="TypeName">Select Type:</label>
                        <select name="TypeName" id="TypeName">
                            <option value="All">All</option>
                            <option value="Upcomming">Upcomming</option>
                            <option value="Running">Running</option>
                            <option value="Previous">Previous</option>
                            
                        </select>
                        <input type="submit" value="Submit" name="ins" />
                    </form>
                <!-- End -->
                </div>
                
                <br />
                <table id="ranklist">
                  <tr>
                    <th class="tableHwidth">Id</th>
                    <th class="tableHwidth">Name</th>
                    <th>Start Time</th>
                    <th>End Time</th>
                    <th>Total Questions</th>
                    <th>Created By</th>
                    <th>-</th>
                  </tr>
                  <?php
                    if(isset($_GET["ins"]))
                    {
                    $extra="";
                    date_default_timezone_set("Asia/Dhaka");
                    $date = date('Y-m-d H:i:s');
                    $type = $_GET['TypeName'];
                    
                    if($type=="Upcomming")
                    {
                        $extra = "and ct.start_time>'".$date."'";
                    }
                    else if($type=="Running")
                    {
                        $extra = "and ct.start_time<='".$date."' and ct.end_time>='".$date."'";
                    }
                    else if($type=="Previous")
                    {
                        $extra = "and ct.end_time<'".$date."'";
                    }
                    //$extra="";
                    $qry = "SELECT ct.*,ut.name FROM CreateTournament ct,UserTable ut WHERE ct.created_by=ut.id $extra";
                    //echo $qry;
                    $check = $conn->prepare($qry);
                    $check->execute();
                    $check_cnt = $check->rowCount();
                    $results = $check->fetchAll();
                   
                    for($i=0;$i<$check_cnt;$i++){
                        $id = $results[$i][0];
                        $name = $results[$i][1];
                        $startTime = $results[$i][3];
                        $endTime = $results[$i][4];
                        $rank = $results[$i][0];
                        $total = $results[$i][2];
                        $createdBy = $results[$i][7];
                        echo "<tr>
                        <td>$id</td>
                        <td>$name</td>
                        <td>$startTime</td>
                        <td>$endTime</td>
                        <td>$total</td>
                        <td>$createdBy</td>";
                        if($date>$endTime)
                        {
                            echo $date;
                            echo "<td><a href='privateRank.php?id=$id'>Rank List</td>";
                        }
                        else
                        {
                            echo $endTime;
                            echo "<td><a href='privatePassword.php?id=$id'>Enter</td>";
                        }
                      echo "</tr>";
                    }
                    }
                    else
                    {
                    date_default_timezone_set("Asia/Dhaka");
                    $date = date('Y-m-d H:i:s');
                    $qry = "SELECT ct.*,ut.name FROM CreateTournament ct,UserTable ut WHERE ct.created_by=ut.id";
                    $check = $conn->prepare($qry);
                    $check->execute();
                    $check_cnt = $check->rowCount();
                    $results = $check->fetchAll();
                   
                    for($i=0;$i<$check_cnt;$i++){
                        $id = $results[$i][0];
                        $name = $results[$i][1];
                        $startTime = $results[$i][3];
                        $endTime = $results[$i][4];
                        $rank = $results[$i][0];
                        $total = $results[$i][2];
                        $createdBy = $results[$i][7];
                        echo "<tr>
                        <td>$id</td>
                        <td>$name</td>
                        <td>$startTime</td>
                        <td>$endTime</td>
                        <td>$total</td>
                        <td>$createdBy</td>";
                        if($date>$endTime)
                        {
                            echo "<td><a href='privateRank.php?id=$id'>Rank List</td>";
                        }
                        else
                        {
                            echo "<td><a href='privatePassword.php?id=$id'>Enter</td>";
                        }
                      echo "</tr>";
                    }
                    }
                  ?>
                </table>
            </div>
        </div>
      </div>
      <!-- // Item -->
    </div>
</div>
<!-- ***** Main Banner Area End ***** -->

<?php
    require 'user-footer.php';
?>