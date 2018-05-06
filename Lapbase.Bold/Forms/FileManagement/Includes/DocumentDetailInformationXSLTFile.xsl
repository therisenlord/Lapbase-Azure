<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt">

<xsl:template match="/">
  <html>
    <body >
		
		<input type="button" id = "btnDelete"  onclick="javascript:DeleteDocument(this);" >
      <xsl:if test ="/dsSchema/tblDocumentDetailInformation/PermissionLevel = '1o' or /dsSchema/tblDocumentDetailInformation/PermissionLevel = '2t' or /dsSchema/tblDocumentDetailInformation/PermissionLevel = '3f' or /dsSchema/tblDocumentDetailInformation/PermissionLevel = '4s'">
        <xsl:attribute name ="style">display:none</xsl:attribute>
      </xsl:if>      
			<xsl:for-each select="/dsSchema/tblCaptions">
				<xsl:if test ="Field_ID = 'btnDelete'">
					<xsl:attribute name ="value"><xsl:value-of select="FIELD_CAPTION"/></xsl:attribute>
				</xsl:if>
			</xsl:for-each>
		</input>
		<xsl:if test="/dsSchema/tblDocumentDetailInformation/EventLink != 'B'">
			<div class="operations">
			  <h3>
				<xsl:value-of select ="/dsSchema/tblDocumentDetailInformation/EventName"/>&#160;<xsl:value-of select ="/dsSchema/tblDocumentDetailInformation/EventDate"/>
			  </h3>
			  <div class="operationDetails">
				<p><a href="#">
					<xsl:attribute name="onclick">
					  javascript:LoadEventData(<xsl:value-of select="/dsSchema/tblDocumentDetailInformation/tblPatientDocumentsID"/>, '<xsl:value-of select="/dsSchema/tblDocumentDetailInformation/EventLink"/>');
					</xsl:attribute>
					<img src="../../img/button_plus.gif" id = "imgEventDetails" width="13" height="13" border="0">
					  <span><xsl:value-of select="/dsSchema/tblDocumentDetailInformation/EventName"/>&#160;
						  <label id ="lblDetails">
							  <xsl:for-each select="/dsSchema/tblCaptions">
								  <xsl:if test ="Field_ID = 'lblDetails'">
									  <xsl:value-of select="FIELD_CAPTION"/>
								  </xsl:if>
							  </xsl:for-each>
						  </label>
					</span>
					</img>
				  </a>
				</p>
				<div id ="div_EventDetail" style="display:none"></div>
			  </div>
			</div>
		</xsl:if>

      <div class="imgDisplayWrap">
        <div class="imgDisplay">
          <h3>
            <xsl:value-of select="/dsSchema/tblDocumentDetailInformation/DocumentName"/>&#160;
            <label style="Color:#DD0000;" id ="lblDeleted">
              <xsl:if test = "/dsSchema/tblDocumentDetailInformation/IsDeleted = 'true'">&lt;
				  <label id ="lblDeletedReadOnly">
					  <xsl:for-each select="/dsSchema/tblCaptions">
						  <xsl:if test ="Field_ID = 'lblDetails'">
							  <xsl:value-of select="FIELD_CAPTION"/>
						  </xsl:if>
					  </xsl:for-each>
				  </label>&gt;</xsl:if>
            </label>
          </h3>

          <div class="imgWrap">
            <div class="imgShadow">
              <img src="../../img/test_image_place_holder.jpg" width="86" height="98"></img>
            </div>
            <p>
              <a href="#" alt="Show larger version" id ="linkLarge">
                <xsl:attribute name="onclick">
                  setDocumentName('<xsl:value-of select="/dsSchema/tblDocumentDetailInformation/DocumentFileName"/>');linkLarge_onclick(<xsl:value-of select="/dsSchema/tblDocumentDetailInformation/tblPatientDocumentsID"/>);
                </xsl:attribute>
				  <xsl:for-each select="/dsSchema/tblCaptions">
					  <xsl:if test ="Field_ID = 'linkLarge'"><xsl:value-of select="FIELD_CAPTION"/></xsl:if>
				  </xsl:for-each>
              </a>
            </p>
          </div>
          <div class="txtWrap">
            <table class ="imgDisplayFrm">
              <tr>
                <td>
                  <label >
                    <span id ="lblLabel">
						<xsl:for-each select="/dsSchema/tblCaptions">
							<xsl:if test ="Field_ID = 'lblLabel'">
								<xsl:value-of select="FIELD_CAPTION"/>
							</xsl:if>
						</xsl:for-each></span>
                  </label>
                </td>
                <td>
                  <input name="txtDocumentName" id="txtDocumentName" type="text" >
                    <xsl:if test ="/dsSchema/tblDocumentDetailInformation/IsDeleted = 'true'">
                      <xsl:attribute name="disabled">true</xsl:attribute>
                    </xsl:if>
                    <xsl:attribute name="value">
                      <xsl:value-of select="/dsSchema/tblDocumentDetailInformation/DocumentName"/>
                    </xsl:attribute>
                  </input>
                </td>
              </tr>
              <tr>
                <td>
                  <label for="txtDoc_Description">
                    <span id ="lblDescription">
						<xsl:for-each select="/dsSchema/tblCaptions">
							<xsl:if test ="Field_ID = 'lblDescription'">
								<xsl:value-of select="FIELD_CAPTION"/>
							</xsl:if>
						</xsl:for-each></span>
                  </label>
                </td>
                <td>
                  <textarea name="txtDoc_Description" id="txtDoc_Description" rows="3" >
                    <xsl:if test ="IsDeleted = 'true'">
                      <xsl:attribute name="disabled">true</xsl:attribute>
                    </xsl:if>
                    <xsl:value-of select="/dsSchema/tblDocumentDetailInformation/Doc_Description"/>
                  </textarea>
                </td>
              </tr>
            </table>
            <input class="subBtn" id ="btnSaveDocument" value="Save" type="button" >
              <xsl:if test ="/dsSchema/tblDocumentDetailInformation/PermissionLevel = '1o'">
                <xsl:attribute name ="style">display:none</xsl:attribute>
              </xsl:if>
              
				<xsl:if test ="/dsSchema/tblDocumentDetailInformation/IsDeleted = 'true'">
					<xsl:attribute name="disabled">true</xsl:attribute>
				</xsl:if>
				<xsl:attribute name="onclick">
					javascript:btnSaveDocument_onclick(<xsl:value-of select="/dsSchema/tblDocumentDetailInformation/tblPatientDocumentsID"/>);
				</xsl:attribute>
				<xsl:for-each select="/dsSchema/tblCaptions">
					<xsl:if test ="Field_ID = 'btnSaveDocument'">
						<xsl:attribute name="value"><xsl:value-of select="FIELD_CAPTION"/></xsl:attribute>
					</xsl:if>
				</xsl:for-each>
            </input>
          </div>

          <div class="clr"></div>
        </div>
      </div>
      
      <div class="clr"></div>
      
      <div class="innerTabs">
      <div class="innerManilaTabMenu">
        <ul>
          <li class="current" id ="l_FileDetails">
            <a href="#" id="a_FileDetials" >
				<xsl:attribute name="onclick">
					javascript:LoadFileDetailData("div_FileDetails", <xsl:value-of select="/dsSchema/tblDocumentDetailInformation/tblPatientDocumentsID"/>);
				</xsl:attribute>
				<xsl:for-each select="/dsSchema/tblCaptions">
					<xsl:if test ="Field_ID = 'a_FileDetials'">
						<xsl:value-of select="FIELD_CAPTION"/>
					</xsl:if>
				</xsl:for-each>
			</a>
          </li>
          <li id ="l_FileHistory">
            <a href="#" id="a_FileHistory" >
				<xsl:attribute name="onclick">
					javascript:LoadFileDetailData("div_FileHistory", <xsl:value-of select="/dsSchema/tblDocumentDetailInformation/tblPatientDocumentsID"/>);
				</xsl:attribute>
				<xsl:for-each select="/dsSchema/tblCaptions">
					<xsl:if test ="Field_ID = 'a_FileHistory'">
						<xsl:value-of select="FIELD_CAPTION"/>
					</xsl:if>
				</xsl:for-each>
			</a>
          </li>
        </ul>
      </div>
      <div class="innerTabsContent">
        <div class="fileDetailsHistory" id ="div_FileDetails" >
          <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tbody>
              <tr>
                <td width="24%" id ="lblFileType">
                  <strong>
					  <xsl:for-each select="/dsSchema/tblCaptions">
						  <xsl:if test ="Field_ID = 'lblFileType'">
							  <xsl:value-of select="FIELD_CAPTION"/>
						  </xsl:if>
					  </xsl:for-each></strong>
                </td>
                <td width="76%">
                  <xsl:value-of select="substring-after(/dsSchema/tblDocumentDetailInformation/DocumentFileName,'.')"/>
                </td>
              </tr>
              <tr>
                <td id ="lblOriginalFileName">
                  <strong>
					  <xsl:for-each select="/dsSchema/tblCaptions">
						  <xsl:if test ="Field_ID = 'lblOriginalFileName'">
							  <xsl:value-of select="FIELD_CAPTION"/>
						  </xsl:if>
					  </xsl:for-each></strong>
                </td>
                <td>
                  <xsl:value-of select="/dsSchema/tblDocumentDetailInformation/DocumentFileName"/>
                </td>
              </tr>
              <tr>
                <td id ="lblFileSize">
                  <strong>
					  <xsl:for-each select="/dsSchema/tblCaptions">
						  <xsl:if test ="Field_ID = 'lblFileSize'">
							  <xsl:value-of select="FIELD_CAPTION"/>
						  </xsl:if>
					  </xsl:for-each></strong>
                </td>
                <td>
                  <xsl:value-of select="round(/dsSchema/tblDocumentDetailInformation/DocumentFileSize div 1024)"/>&#160; KB
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <div id ="div_FileHistory" style="display:none;overflow:auto;height:75px"></div>
      </div>
    </div>
    </body>
  </html>
</xsl:template>

</xsl:stylesheet> 

