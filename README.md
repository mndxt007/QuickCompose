# 🚀 Quick Compose – Your Outlook Productivity Co-Pilot

> Supercharge your Outlook experience by turning your inbox chaos into a prioritized action plan – powered by LLMs.

## ✨ Why This Exists

This project was built during the early emergence of Large Language Models (LLMs), before Microsoft 365 Co-pilot or similar solutions had matured. The goal? Provide a powerful, lightweight assistant that streamlines your email triage and response workflow — directly inside Outlook.

## 🧠 The Problem

If you're in a role that revolves around email, you know the drill:

- You open your inbox to **hundreds of unread emails**, each needing attention.
- You spend precious time **planning your day**, manually figuring out what's urgent and what can wait.
- You draft repetitive responses all day long.

### Imagine if:
- You opened Outlook and instantly saw **your top priorities**—already **analyzed, categorized, and summarized**.
- Responses were **auto-drafted** and ready for your review.
- The system could **adapt** to your organization’s workflow, categories, and even plug into other tools like Git or DFM.

## ✅ The Solution

**Quick Compose** is an intelligent Outlook add-in designed to make your inbox *work for you*. It brings AI-driven insights right into your daily workflow:

- 📬 **In-context Assistance**: Launches in the Outlook sidebar, pre-generates replies, or works with your custom prompts.
- 🧠 **Smart Email Analysis**: Automatically categorizes emails based on **sentiment**, **urgency**, and **actionability**.
- ✍️ **Pre-composed Responses**: Review, tweak, and send AI-drafted replies — saving you time and reducing cognitive load.
- ⚙️ **Customizable Integration**: Connect to your preferred LLM endpoint, define your own email categories, and extend it to work with task management tools like Git or DFM.

## 🖼️ Screenshots

![1](assets/1.png)
![2](assets/2.png) 
![3](assets/3.png) 
![4](assets/4.png) 

## 🧩 Project Breakdown

| Module | Description |
|--------|-------------|
| [AnonymizerAPI](AnonymizerAPI) | Presidio-based API that redacts PII from email content before sending it to an LLM endpoint. |
| [outlook-blazor-addin](outlook-blazor-addin) | Blazor web app hosted as an Outlook Add-in. Interfaces with the Outlook JS API for email context. |
| [outlook-blazor-sideloader](outlook-blazor-sideloader) | Project to sideload the add-in into Outlook Web. |
| [Desktop Client](OutlookMAUI8) | Native desktop client for Outlook Classic. Uses a COM Add-in for integration. |

## 🔗 References & Inspiration

- [📘 Blazor Outlook Add-in Sample](https://github.com/OfficeDev/Office-Add-in-samples/tree/main/Samples/blazor-add-in/outlook-blazor-add-in)
- [📘 Office.context.mailbox Docs](https://learn.microsoft.com/en-us/javascript/api/requirement-sets/outlook/requirement-set-1.5/office.context.mailbox?view=powerpoint-js-preview)
- [📘 Office.context.mailbox.item](https://learn.microsoft.com/en-us/javascript/api/requirement-sets/outlook/requirement-set-1.5/office.context.mailbox.item?view=powerpoint-js-preview)

---

