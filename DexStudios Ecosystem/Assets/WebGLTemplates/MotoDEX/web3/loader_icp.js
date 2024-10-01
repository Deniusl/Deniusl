import {AuthClient} from "https://cdn.jsdelivr.net/npm/@dfinity/auth-client@1.3.0/+esm";
import {HttpAgent, Actor} from "https://cdn.jsdelivr.net/npm/@dfinity/agent@1.3.0/+esm";
import {AccountIdentifier} from "https://cdn.jsdelivr.net/npm/@dfinity/ledger-icp@2.2.3/+esm";
import {Principal} from "https://cdn.jsdelivr.net/npm/@dfinity/principal@1.3.0/+esm";
import fetch from 'https://cdn.jsdelivr.net/npm/isomorphic-fetch@3.0.0/+esm';

window.HttpAgent = HttpAgent;
window.Actor = Actor;
window.AccountIdentifier = AccountIdentifier;
window.Principal = Principal;

import {idlFactory as idlFactoryMotodex} from "/motodex.did.js";
import {idlFactory as idlFactoryICP} from "/icp.did.js";

window.idlFactoryMotodex = idlFactoryMotodex;
window.idlFactoryICP = idlFactoryICP;

async function initializeAuthClient() {
    try {
        const ICPauthClient = await AuthClient.create();
        window.ICPauthClient = ICPauthClient;
        console.log("AuthClient initialized successfully.");
    } catch (err) {
        console.error("Error initializing AuthClient:", err);
    }
}

const host = 'https://icp-api.io'; // Verify this endpoint

const agent = new window.HttpAgent({
    fetch,
    host,
});

async function fetchRootKey() {
    try {
        await agent.fetchRootKey();
        console.log("Root key fetched successfully.");
    } catch (err) {
        console.warn("Unable to fetch root key. Check to ensure that your local replica is running");
        console.error("Error details:", err);
        if (err.response) {
            const errorBody = await err.response.text();
            console.error("Response body:", errorBody);
        }
    }
}

async function initializeActors() {
    const actorMotodexReadOnly = window.Actor.createActor(window.idlFactoryMotodex, {
        agent,
        canisterId: "be2us-64aaa-aaaaa-qaabq-cai"
    });
    window.actorMotodexReadOnly = actorMotodexReadOnly;

    try {
        const get_counter = await window.actorMotodexReadOnly.get_counter();
        console.log("window.actorMotodexReadOnly get_counter " + get_counter);
    } catch (err) {
        console.error("Error fetching counter:", err);
        if (err.response) {
            const errorBody = await err.response.text();
            console.error("Response body:", errorBody);
        }
    }

    const actorICPReadOnly = window.Actor.createActor(window.idlFactoryICP, {
        agent,
        canisterId: "ryjl3-tyaaa-aaaaa-aaaba-cai"
    });
    window.actorICPReadOnly = actorICPReadOnly;

    try {
        const decimals = await window.actorICPReadOnly.decimals();
        console.log("window.actorICPReadOnly decimals " + JSON.stringify(decimals));
    } catch (err) {
        console.error("Error fetching decimals:", err);
        if (err.response) {
            const errorBody = await err.response.text();
            console.error("Response body:", errorBody);
        }
    }
}

async function initialize() {
    await initializeAuthClient();
    await fetchRootKey();
    await initializeActors();
}

initialize();