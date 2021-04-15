<template>
  <v-container fluid>
    <h1>How to use</h1>
    <v-row>
      <v-col><p>This tool provides a web interface, available on the network, to quickly change information displayed on a live TCG stream. 
        You can set up image sources pointing to :
          <code>[install directory]/activeImage/current.[jpg/png/gif]</code>
        for displaying a featured card, and text sources pointing to
          <code>[install directory]/players/player[Left/Right]_[name/deck/score].txt</code>
        for displaying basic infos about the players.
        </p></v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component({
  components: {
  },
})
export default class Home extends Vue {}
</script>
