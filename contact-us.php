<?php
    require 'user-header.php';
?>

    <!-- ***** Main Banner Area Start ***** -->
    <div class="main-banner header-text" id="top">
    </div>
    <!-- ***** Main Banner Area End ***** -->
    
 <!-- ***** Contact Us Area Starts ***** -->
 <section class="section" id="contact-us">
    <div class="container">
        <div class="row">
            <div class="col-lg-4 col-md-4 col-xs-12">
                <div class="left-text-content">
                    <div class="section-heading">
                        <h6>Contact Us</h6>
                        <h2>Feel Free to Contuct With Us!</h2>
                    </div>
                    <ul class="contact-info">
                        <li><img src="assets/images/contact-info-01.png" alt="">+8801816570051</li>
                        <li><img src="assets/images/contact-info-02.png" alt="">jubayersohel4@purnochinta.com</li>
                        <li><img src="assets/images/contact-info-03.png" alt="">www.purnochinta.com</li>
                    </ul>
                </div>
            </div>
            <div class="col-lg-8 col-md-8 col-xs-12">
                <div class="contact-form">
                    <form id="contact" action="" method="get">
                      <div class="row">
                        <div class="col-md-6 col-sm-12">
                          <fieldset>
                            <input name="name" type="text" id="name" placeholder="Your Name *" required="">
                          </fieldset>
                        </div>
                        <div class="col-md-6 col-sm-12">
                          <fieldset>
                            <input name="phone" type="text" id="phone" placeholder="Your Phone" required="">
                          </fieldset>
                        </div>
                        <div class="col-md-6 col-sm-12">
                          <fieldset>
                            <input name="email" type="email" id="email" placeholder="Your Email *" required="">
                          </fieldset>
                        </div>
                        <div class="col-md-6 col-sm-12">
                          <fieldset>
                            <input name="subject" type="text" id="subject" placeholder="Subject">
                          </fieldset>
                        </div>
                        <div class="col-lg-12">
                          <fieldset>
                            <textarea name="message" rows="6" id="message" placeholder="Message" required=""></textarea>
                          </fieldset>
                        </div>
                        <div class="col-lg-12">
                          <fieldset>
                            <button type="submit" id="form-submit" class="main-button-icon">Send Message Now <i class="fa fa-arrow-right"></i></button>
                          </fieldset>
                        </div>
                      </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</section>
<!-- ***** Contact Us Area Ends ***** -->
    
<?php
        require 'user-footer.php';
    ?> 