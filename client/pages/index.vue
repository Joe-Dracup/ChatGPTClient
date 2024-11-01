<template>
   <div class="max-w-md mx-auto mt-8">
        <FormKit 
        v-model="message"
        type="text" 
        name="textbox" 
        id="message" 
        label="
         What do you need help with?"
        />

        <FormKit
        v-model="context"
        type="select"
        label="What is the context"
        name="context"
        id="context"
        placeholder="Please select the context"
        :options="['CallCenterHelp', 'CustomerInformation', 'Test']"
        />

        <FormKit
        type="button" 
        name="search" 
        id="Search" 
        label=" Click to search"
        @click="Call"
        />
        <h1> {{ response }}</h1>
    </div>

    
</template>

<script setup lang="ts">
const message = ref('hello')

const context = ref('CallCenterHelp')

 const response = ref('hello')
 console.log(response)
console.log(response.value)

async function Call(event) {
  response.value =  (await $fetch('http://localhost:5133/chat', {
    method: 'POST',
    body: { message: message.value, context: context.value}
  }) )
  .choices[0].message.content
 
 
  console.log(response.value);
 
}


</script>