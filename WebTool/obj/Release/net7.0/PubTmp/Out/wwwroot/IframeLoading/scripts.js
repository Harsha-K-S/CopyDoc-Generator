const powerBIUrls = [
  "Select URL",
  "https://powerbi.microsoft.com/en-us/lead-collection/",
  "https://tip1.powerbi.microsoft.com/en-us/lead-collection/",
  "https://tip2.powerbi.microsoft.com/en-us/lead-collection/",
  "https://powerbi-frontend-tip1-eastus.azurewebsites.net/en-us/lead-collection/",
  "https://powerbi-frontend-tip2-eastus.azurewebsites.net/en-us/lead-collection/",
];

const powerAppsUrls = [
  "Select URL",
  "https://powerapps.microsoft.com/en-us/lead-collection/",
  "https://tip1.powerapps.microsoft.com/en-us/lead-collection/",
  "https://tip2.powerapps.microsoft.com/en-us/lead-collection/",
  "https://powerapps-frontend-tip1-eastus.azurewebsites.net/en-us/lead-collection/",
  "https://powerapps-frontend-tip2-eastus.azurewebsites.net/en-us/lead-collection/",
];

const powerBIOffer = "CO-DBI-ACCOUNT-POWERBI-TRIALINITIATION-OFFERID";
const powerAppsOffer = "CO-RAPIDAPPS-ACCOUNT-POWERAPPS-FRE-OFFERID";

init();

function init() {
  $(document).ready(function () {
    populateSelectOptions("powerbi-url-selector", powerBIUrls);
    populateSelectOptions("powerbi-offer-selector", [
      powerBIOffer,
      powerAppsOffer,
    ]);
    populateSelectOptions("powerapps-url-selector", powerAppsUrls);
    populateSelectOptions("powerapps-offer-selector", [
      powerAppsOffer,
      powerBIOffer,
    ]);
    $("#powerbi-btn-load").on("click", () => {
      onPowerBILoad(true);
    });
    $("#powerbi-btn-other").on("click", onPowerBIOther);
    $("#powerbi-btn-send").on("click", onPowerBISend);
    $("#powerapps-btn-load").on("click", () => {
      onPowerAppsLoad(true);
    });
    $("#powerapps-btn-other").on("click", onPowerAppsOther);
    $("#powerapps-btn-send").on("click", onPowerAppsSend);
    $("#powerbi-url-selector").on("change", () => {
      onPowerBISelectorChanged();
      onPowerBILoad(false);
    });
    $("#powerbi-offer-selector").on("change", onPowerBISelectorChanged);
    $("#powerapps-url-selector").on("change", () => {
      onPowerAppsSelectorChanged();
      onPowerAppsLoad(false);
    });
    $("#powerapps-offer-selector").on("change", onPowerAppsSelectorChanged);
    $(".nav-link").on("click", () => {
      onPowerBISelectorChanged();
      onPowerAppsSelectorChanged();
    });
    $(".accordion-button").on("click", () => {
      onPowerBISelectorChanged();
      onPowerAppsSelectorChanged();
    });
    onPowerBISelectorChanged();
    onPowerAppsSelectorChanged();
  });
}

function onPowerBISelectorChanged() {
  let offer = document.getElementById("powerbi-offer-selector").value;
  document.getElementById("powerbi-btn-load").disabled = offer != powerBIOffer;
  document.getElementById("powerbi-btn-send").disabled = true;
  hideIFrame();
}

function onPowerAppsSelectorChanged() {
  let offer = document.getElementById("powerapps-offer-selector").value;
  document.getElementById("powerapps-btn-load").disabled =
    offer != powerAppsOffer;
  document.getElementById("powerapps-btn-send").disabled = true;
  hideIFrame();
}

function onPowerBILoad(customUrl) {
  let url = customUrl
    ? document.getElementById("powerbi-url-input").value
    : document.getElementById("powerbi-url-selector").value;
  let offer = document.getElementById("powerbi-offer-selector").value;
  if (offer === powerBIOffer && isValidUrl(url)) {
    document.getElementById("powerbi-btn-send").disabled = true;
    showIFrame(url, () => {
      document.getElementById("powerbi-btn-send").disabled = false;
    });
  }
}

function onPowerBIOther() {
  document.getElementById("powerbi-btn-load").style.display = "block";
  document.getElementById("powerbi-btn-other").disabled = true;
  document.getElementById("powerbi-url-input").style.display = "block";
}

function onPowerBISend() {
  sendIFrameMessage({
    token: "",
    userObjId: "88f3267d-b758-4df4-a4e3-562a5efe463a",
    headerText: "Submit a request",
    subHeaderText: "Please provide your details to get started.",
    ctaText: "Submit",
    tenantID: "fad0ccf1-1048-4094-887d-c9a3ff222bd9",
    offerID: document.getElementById("powerbi-offer-selector").value,
    GraphUrl: "https://graph.microsoft.com/",
    userDetails: {
      givenName: "test",
      surname: "test",
      mail: "test@qa.com",
      companyName: "test",
      companySize: "1",
      jobTitle: "test",
      userObjId: "88f3267d-b758-4df4-a4e3-562a5efe463a",
    },
  });
}

function onPowerAppsLoad(customUrl) {
  let url = customUrl
    ? document.getElementById("powerapps-url-input").value
    : document.getElementById("powerapps-url-selector").value;
  let offer = document.getElementById("powerapps-offer-selector").value;
  if (offer === powerAppsOffer && isValidUrl(url)) {
    document.getElementById("powerapps-btn-send").disabled = true;
    showIFrame(url, () => {
      document.getElementById("powerapps-btn-send").disabled = false;
    });
  }
}

function onPowerAppsOther() {
  document.getElementById("powerapps-btn-load").style.display = "block";
  document.getElementById("powerapps-btn-other").disabled = true;
  document.getElementById("powerapps-url-input").style.display = "block";
}

function onPowerAppsSend() {
  sendIFrameMessage({
    token: "",
    userObjId: "88f3267d-b758-4df4-a4e3-562a5efe463a",
    headerText: "Submit a request",
    subHeaderText: "Please provide your details to get started.",
    ctaText: "Submit",
    tenantID: "fad0ccf1-1048-4094-887d-c9a3ff222bd9",
    offerID: document.getElementById("powerapps-offer-selector").value,
    GraphUrl: "https://graph.microsoft.com/",
    userDetails: {
      givenName: "test",
      surname: "test",
      mail: "test@qa.com",
      companyName: "test",
      companySize: "1",
      jobTitle: "test",
      userObjId: "88f3267d-b758-4df4-a4e3-562a5efe463a",
    },
  });
}

function populateSelectOptions(selectId, values) {
  var select = document.getElementById(selectId);
  values.forEach(
    (v) => (select.options[select.options.length] = new Option(v, v))
  );
}

function showIFrame(src, onload) {
  document.getElementById("iframe-container").style.display = "inline-block";
  document.getElementById("iframe").src = src;
  document.getElementById("iframe").onload = () => {
    document.getElementById("iframe-loading").style.display = "none";
    document.getElementById("iframe-loaded").style.display = "inline-block";
    onload();
    document.getElementById("iframe").onload = null;
  };
  document.getElementById("iframe-loading").style.display = "inline-block";
  document.getElementById("iframe-loaded").style.display = "none";
}

function hideIFrame() {
  document.getElementById("iframe-container").style.display = "none";
  document.getElementById("iframe").src = "about:blank";
  document.getElementById("iframe-loading").style.display = "inline-block";
  document.getElementById("iframe-loaded").style.display = "none";
}

function sendIFrameMessage(message) {
  document
    .getElementById("iframe")
    .contentWindow.postMessage(JSON.stringify(message), "*");
}

function isValidUrl(string) {
  try {
    new URL(string);
    return true;
  } catch (err) {
    return false;
  }
}
window.addEventListener("message", (event) => {
  console.log("sddsdd", event.data);
  // Do we trust the sender of this message?
  if (event.origin !== "http://example.com:8080") return;

  // event.source is window.opener
  // event.data is "hello there!"

  // Assuming you've verified the origin of the received message (which
  // you must do in any case), a convenient idiom for replying to a
  // message is to call postMessage on event.source and provide
  // event.origin as the targetOrigin.
  event.source.postMessage(
    "hi there yourself!  the secret response " + "is: rheeeeet!",
    event.origin
  );
});
