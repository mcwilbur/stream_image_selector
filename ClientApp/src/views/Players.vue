<template>
  <v-container fluid>    
    <v-row>
      <v-col sm="6" xs="12" cols="12" >
        <v-container>
          <v-row><v-col><h2>Left Player</h2></v-col></v-row>
          <v-row><v-col><v-text-field label="Player name" v-model="playerLeft.name"/></v-col></v-row>
          <v-row><v-col><v-text-field label="Deck" v-model="playerLeft.deck"/></v-col></v-row>
          <v-row><v-col><v-text-field label="Score" v-model="playerLeft.score"/></v-col></v-row>
          </v-container>
      </v-col>
      <v-col sm="6" xs="12" cols="12">
        <v-container>
          <v-row><v-col><h2>Right Player</h2></v-col></v-row>
          <v-row><v-col><v-text-field label="Player name" v-model="playerRight.name"/></v-col></v-row>
          <v-row><v-col><v-text-field label="Deck" v-model="playerRight.deck"/></v-col></v-row>
          <v-row><v-col><v-text-field label="Score" v-model="playerRight.score"/></v-col></v-row>
          </v-container>
      </v-col>
    </v-row> 
    <v-btn color="success" class="mr-4" fixed top right style="top: 80px" v-on:click="setPlayers">Update</v-btn>   
  </v-container>  
</template>

<script lang="ts">
// an example of a Vue Typescript component using Vue.extend
import Vue from 'vue';
import axios from 'axios';

export default Vue.extend({
  data() {
    return {
      loading: true,
      showError: false,
      errorMessage: 'Error while loading players.',
      playerLeft: {
        name: '',
        deck: '',
        score: '',
      },
      playerRight: {
        name: '',
        deck: '',
        score: '',
      },
    };
  },
  methods: {
    async fetchPlayers() {
      try {
        const response = await axios.get('api/Players');
        this.playerLeft = response.data.playerLeft;
        this.playerRight = response.data.playerRight;
      } catch (e) {
        this.showError = true;
        this.errorMessage = `Error while loading players: ${e.message}.`;
      }
      this.loading = false;
    },
    async setPlayers() {
      try {
        const response = await axios.post('api/Players', {playerLeft : this.playerLeft, playerRight: this.playerRight});
        await this.fetchPlayers();
      } catch (e) {
        this.showError = true;
        this.errorMessage = `Error while setting player info ${e.message}.`;
      }
    },
  },
  async created() {
    await this.fetchPlayers();
  },
});
</script>
<style scoped>
  .active{
    border-color:  rgba(82,168,236,.8);
    box-shadow: 0 0px 0px rgba(82,168,236,.8) inset, 0 0 8px rgba(82,168,236,.8);
    outline: 0 none;
  }
</style>
