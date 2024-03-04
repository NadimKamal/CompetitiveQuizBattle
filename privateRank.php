<?php
    require 'user-header.php';
    session_start();
    $user_id = $_SESSION["user_id"];
    $id = $_GET["id"];
    
    
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
                
                
                <table id="ranklist">
                
                  <tr>
                    <th class='rank'>Rank</th>
                    <th>Name</th>
                    <th>Tournament Name</th>
                    <th>Points</th>
                  </tr>
                  <?php
                        $qry = "SELECT ut.name,nt.private_name,score FROM CreateTournament nt,UserTable ut,private_result pr WHERE pr.user_id=ut.id AND pr.private_id=nt.private_id and pr.private_id=".$id." ORDER BY score DESC";
                        //echo $qry;
                        $check = $conn->prepare($qry);
                        $check->execute();
                        $check_cnt = $check->rowCount();
                        $results = $check->fetchAll();
                        $one = 1;
                        for($i=0;$i<$check_cnt;$i++){
                            $name = $results[$i][0];
                            $count = $results[$i][2];
                            $inst = $results[$i][1];
                            
                          echo "<tr>
                            <td class='rank'>$one</td>
                            <td>$name</td>
                            <td>$inst</td>
                            <td>$count</td>
                          </tr>";
                          $one++;
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